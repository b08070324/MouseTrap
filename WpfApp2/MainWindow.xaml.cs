using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WpfApp2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// Datagrid source
		// Using a temp list to aggregate fresh data before updating the datagrid collection
		private List<WindowInformation> TempWindowInfo { get; set; }
		private BatchedObservableCollection<WindowInformation> WindowInfo { get; set; }
		private CollectionViewSource CollectionViewSource { get; set; }

		// To allow updates to UI thread from worker thread
		// Can use either SynchronizationContext and SynchronizationContext.Send()
		// or BindingOperations.EnableCollectionSynchronization with a larger perf overhead
		private SynchronizationContext uiContext = SynchronizationContext.Current;

		public MainWindow()
		{
			InitializeComponent();

			// Bind collection to datagrid
			TempWindowInfo = new List<WindowInformation>();
			WindowInfo = new BatchedObservableCollection<WindowInformation>();
			CollectionViewSource = new CollectionViewSource();
			CollectionViewSource.Source = WindowInfo;
			CollectionViewSource.Filter += CollectionViewSource_Filter;
			outputGrid.ItemsSource = CollectionViewSource.View;
			outputGrid.SelectedCellsChanged += OutputGrid_SelectedCellsChanged;
		}

		private void OutputGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
            // Return if no cells selected
			if (e.AddedCells.Count < 1) return;

            // Get selected item
            var item = e.AddedCells.First().Item as WindowInformation;
			if (item == null) return;

            // Update display
            selectedTitle.Text = string.Format("{0} - {1}", item.ProcessName, item.Name);
            selectedTop.Text = item.Top.ToString();
			selectedLeft.Text = item.Left.ToString();
			selectedWidth.Text = (item.Right- item.Left).ToString();
			selectedHeight.Text = (item.Bottom - item.Top).ToString();
		}

		private void ProcessNameInput_TextChanged(object sender, TextChangedEventArgs e)
		{
			CollectionViewSource.View.Refresh();
		}

		private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
		{
			var item = e.Item as WindowInformation;
            var titleMatches = item.Name.ToLower().Contains(processNameInput.Text);
            var procNameMatches = item.ProcessName.ToLower().Contains(processNameInput.Text);
            e.Accepted = titleMatches || procNameMatches;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			UpdateWindowInformation();
		}

		private void UpdateWindowInformation()
		{
			// Get data
			TempWindowInfo.Clear();
			Win32Interop.EnumWindows(new Win32Interop.WindowEnumCallback(EnumWindowsCallback), 0);

			// Update datagrid
			uiContext.Send(x =>
			{
				WindowInfo.SetItems(TempWindowInfo);
			}
			, null);
		}

		private bool EnumWindowsCallback(IntPtr hWnd, int lParam)
		{
			// Check visibility
			if (!Win32Interop.IsWindowVisible(hWnd) || Win32Interop.IsIconic(hWnd)) return true;

			// Get info
			var info = GetWindowInformation(hWnd);

			// Ignore tool windows
			if (Win32Interop.HasExStyle(info.ExStyle, Win32Interop.WindowStylesEx.WS_EX_TOOLWINDOW)) return true;
			if (Win32Interop.HasExStyle(info.ExStyle, Win32Interop.WindowStylesEx.WS_EX_NOREDIRECTIONBITMAP)) return true;

			// Add to list
			TempWindowInfo.Add(info);

			return true;
		}

		private WindowInformation GetWindowInformation(IntPtr hWnd)
		{
			if (hWnd == null || hWnd == IntPtr.Zero) return null;

			// Get window title
			var title = Win32Interop.GetWindowText(hWnd);

			// Get process info
			Win32Interop.GetWindowThreadProcessId(hWnd, out uint procId);
			var process = Process.GetProcessById((int)procId);

			// Get window size and position
			Win32Interop.GetWindowRect(hWnd, out Win32Interop.Rect rect);

			// Get window ex styles
			var exStyle = Win32Interop.GetWindowLongPtr(hWnd, (int)Win32Interop.GWL.GWL_EXSTYLE);

			// Return object
			return new WindowInformation
			{
				Handle = hWnd,
				Name = title,
				ProcessId = procId,
				ProcessName = process.ProcessName,
				Bottom = rect.Bottom,
				Left = rect.Left,
				Right = rect.Right,
				Top = rect.Top,
				ExStyle = exStyle
			};
		}
	}
}
