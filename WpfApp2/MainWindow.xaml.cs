using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using WpfApp2.Interop;
using WpfApp2.Models;
using WpfApp2.Data;

namespace WpfApp2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// Underlying data
		private string searchText = string.Empty;
		private WindowInformation selectedWindow;
		private bool selectedWindowHasFocus;
		private IntPtr appHandle;

		// View data binding
		private List<WindowInformation> tempWindowList;
		private BatchedObservableCollection<WindowInformation> observedWindowList;
		public CollectionViewSource WindowListViewSource { get; set; }
		public SelectedWindowModel SelectedWindowViewModel { get; set; }

		// Threading
		private SynchronizationContext uiContext = SynchronizationContext.Current;
		private readonly BackgroundWorker worker;
		public bool isPolling;

		// Constructor
		public MainWindow()
		{
			InitializeComponent();

			// View model binding
			tempWindowList = new List<WindowInformation>();
			observedWindowList = new BatchedObservableCollection<WindowInformation>();
			WindowListViewSource = new CollectionViewSource();
			WindowListViewSource.Source = observedWindowList;
			WindowListViewSource.Filter += CollectionViewSource_Filter;
			SelectedWindowViewModel = new SelectedWindowModel();

			// Update thread
			worker = new BackgroundWorker();
			worker.DoWork += worker_DoWork;
			worker.RunWorkerCompleted += worker_RunWorkerCompleted;

			// Continue when app is loaded
			this.Loaded += MainWindow_Loaded;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			// Store a handle to main app window so it can be ignored in list
			appHandle = new System.Windows.Interop.WindowInteropHelper(this).Handle;

			// Update window grid
			RefreshDataGrid();
		}

		// View updates

		private void RefreshDataGrid()
		{
			System.Diagnostics.Debug.WriteLine("Refresh");

			// Reset temp list
			tempWindowList.Clear();

			// Populate temp list
			Win32Interop.EnumWindows(new WindowEnumCallback(EnumWindowsCallback), 0);

			// Update datagrid
			uiContext.Send(x =>
			{
				observedWindowList.SetItems(tempWindowList);
			}
			, null);
		}

		private bool EnumWindowsCallback(IntPtr hWnd, int lParam)
		{
			// Ignore self
			if (hWnd == appHandle) return true;

			// Check visibility
			if (!Win32Interop.IsWindowVisible(hWnd) || Win32Interop.IsIconic(hWnd)) return true;

			// Get info
			var info = new WindowInformation(hWnd);

			// Ignore tool windows
			if (Win32Interop.HasExStyle(info.ExStyle, WindowStylesEx.WS_EX_TOOLWINDOW)) return true;
			if (Win32Interop.HasExStyle(info.ExStyle, WindowStylesEx.WS_EX_NOREDIRECTIONBITMAP)) return true;

			// Add to list
			tempWindowList.Add(info);

			return true;
		}

		private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
		{
			var item = e.Item as WindowInformation;
			var titleMatches = item.Name.ToLower().Contains(searchText);
			var procNameMatches = item.ProcessName.ToLower().Contains(searchText);
			e.Accepted = titleMatches || procNameMatches;
		}

		// Threading

		private void worker_startPolling()
		{
			if (!isPolling)
			{
				isPolling = true;
				worker.RunWorkerAsync();
				System.Diagnostics.Debug.WriteLine("Start polling");
			}
		}

		private void worker_stopPolling()
		{
			isPolling = false;
		}

		// This run in worker thread
		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			// Update underlying data
			selectedWindow.Update();
			var foregroundWindow = Win32Interop.GetForegroundWindow();
			selectedWindowHasFocus = (selectedWindow.Handle == foregroundWindow);

			// Throttle polling
			Thread.Sleep(150);
		}

		// This runs in UI thread
		private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (isPolling)
			{
				// Update view model
				SelectedWindowViewModel.Title = selectedWindow.Name;
				SelectedWindowViewModel.Process = selectedWindow.FullProcessName;
				SelectedWindowViewModel.Top = selectedWindow.Top;
				SelectedWindowViewModel.Left = selectedWindow.Left;
				SelectedWindowViewModel.Width = (selectedWindow.Right - selectedWindow.Left);
				SelectedWindowViewModel.Height = (selectedWindow.Bottom - selectedWindow.Top);
				SelectedWindowViewModel.HasFocus = selectedWindowHasFocus;

				// Execute another poll
				worker.RunWorkerAsync();
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Stop polling");
			}
		}

		// Input event handlers

		private void RefreshWindowList_Click(object sender, RoutedEventArgs e)
		{
			RefreshDataGrid();
		}

		private void OutputGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			// Get selected window
			if (e.AddedCells.Count > 0) selectedWindow = e.AddedCells.First().Item as WindowInformation;
			else selectedWindow = null;

			// Start (or continue) polling if window was selected
			if (selectedWindow != null) worker_startPolling();
			else worker_stopPolling();
		}

		private void ProcessNameInput_TextChanged(object sender, TextChangedEventArgs e)
		{
			var box = e.Source as TextBox;
			searchText = box.Text;
			WindowListViewSource.View.Refresh();
		}
	}
}
