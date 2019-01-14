using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using MouseTrap.Interop;
using MouseTrap.Models;
using MouseTrap.Data;

namespace MouseTrap
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// Underlying data
		private IntPtr appHandle;
		private string searchText = string.Empty;
		private WindowInformation selectedWindow;

		// General app variables for binding etc
		public WindowModel WindowModel { get; set; } = new WindowModel();

		// Datagrid binding
		private List<WindowInformation> tempWindowList = new List<WindowInformation>();
		private BatchedObservableCollection<WindowInformation> observedWindowList = new BatchedObservableCollection<WindowInformation>();
		public CollectionViewSource WindowListViewSource { get; set; } = new CollectionViewSource();

		// Threading
		private SynchronizationContext uiContext = SynchronizationContext.Current;
		private readonly BackgroundWorker worker;
		public bool isPolling;

		// Mouse hook
		private HookProc mouseHookCallback;
		private IntPtr mouseHookPtr;

		// Constructor
		public MainWindow()
		{
			InitializeComponent();
			this.Loaded += MainWindow_Loaded;
			this.Closing += MainWindow_Closing;

			// View model binding
			WindowListViewSource.Source = observedWindowList;
			WindowListViewSource.Filter += CollectionViewSource_Filter;

			// Update thread
			worker = new BackgroundWorker();
			worker.WorkerSupportsCancellation = true;
			worker.DoWork += DoWork;
			worker.RunWorkerCompleted += RunWorkerCompleted;

			// Global mouse hook
			mouseHookCallback = new HookProc(MouseHookCallbackFunction);
			mouseHookPtr = IntPtr.Zero;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			// Store a handle to main app window so it can be ignored in list
			appHandle = new System.Windows.Interop.WindowInteropHelper(this).Handle;

			// Global mouse hook
			HookMouse();

			// Update window grid
			RefreshDataGrid();
		}

		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			StopWorker();
			UnhookMouse();
		}

		// View updates etc

		private void RefreshDataGrid()
		{
			System.Diagnostics.Debug.WriteLine("Refresh");

			// Disable mouse trap
			WindowModel.MouseTrapRequested = false;

			// Reset temp list
			tempWindowList.Clear();

			// Populate temp list
			Win32Interop.EnumWindows(new WindowEnumCallback(EnumWindowsCallback), 0);

			// Update UI etc
			uiContext.Send(x =>
			{
				WindowModel.SelectedIndex = -1;
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

		// Threading

		private void StartWorker()
		{
			if (!isPolling)
			{
				isPolling = true;
				worker.RunWorkerAsync();
				System.Diagnostics.Debug.WriteLine("Start polling");
			}
		}

		private void StopWorker()
		{
			isPolling = false;
			worker.CancelAsync();
		}

		// This run in worker thread
		private void DoWork(object sender, DoWorkEventArgs e)
		{
			if (worker.CancellationPending)
			{
				e.Cancel = true;
			}
			else
			{
				// Update underlying data
				if (selectedWindow.Update())
				{
					var foregroundWindow = Win32Interop.GetForegroundWindow();
					WindowModel.HasFocus = (selectedWindow.Handle == foregroundWindow);

					// Throttle polling
					Thread.Sleep(150);
				}
				else
				{
					StopWorker();
					RefreshDataGrid();
				}
			}
		}

		// This runs in UI thread
		private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (isPolling)
			{
				// Update view model
				WindowModel.Title = selectedWindow.Name;
				WindowModel.Process = selectedWindow.FullProcessName;
				WindowModel.Top = selectedWindow.Top;
				WindowModel.Left = selectedWindow.Left;
				WindowModel.Width = (selectedWindow.Right - selectedWindow.Left);
				WindowModel.Height = (selectedWindow.Bottom - selectedWindow.Top);

				// Execute another poll
				worker.RunWorkerAsync();
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Stop polling");
			}
		}

		// Mouse hook

		private void HookMouse()
		{
			if (mouseHookPtr == IntPtr.Zero)
			{
				var hMod = System.Runtime.InteropServices.Marshal.GetHINSTANCE(typeof(MainWindow).Module);
				mouseHookPtr = Win32Interop.SetWindowsHookEx(HookType.WH_MOUSE_LL, mouseHookCallback, hMod, 0);
			}
		}

		private void UnhookMouse()
		{
			if (mouseHookPtr != IntPtr.Zero) Win32Interop.UnhookWindowsHookEx(mouseHookPtr);
			mouseHookPtr = IntPtr.Zero;
		}

		private IntPtr MouseHookCallbackFunction(int code, IntPtr wParam, IntPtr lParam)
		{
			// Only handle WM_MOUSEMOVE messages
			var isMouseMove = (wParam.ToInt32() == 0x0200); // #define WM_MOUSEMOVE 0x0200

			// Check if message should be handled
			if (code >= 0 && isMouseMove && WindowModel.MouseTrapRequested && WindowModel.HasFocus)
			{
				// Get pointer data
				var mouseInfo = (MSLLHOOKSTRUCT)System.Runtime.InteropServices.Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
				var point = mouseInfo.pt;

				// Limit X
				if (point.X < selectedWindow.Left + WindowModel.TrapMargin) point.X = selectedWindow.Left + WindowModel.TrapMargin;
				else if (point.X > selectedWindow.Right - WindowModel.TrapMargin) point.X = selectedWindow.Right - WindowModel.TrapMargin;

				// Limit Y
				if (point.Y < selectedWindow.Top + WindowModel.TrapMargin) point.Y = selectedWindow.Top + WindowModel.TrapMargin;
				else if (point.Y > selectedWindow.Bottom - WindowModel.TrapMargin) point.Y = selectedWindow.Bottom - WindowModel.TrapMargin;

				// Move cursor
				Win32Interop.SetCursorPos(point.X, point.Y);

				// Done
				return new IntPtr(1);
			}

			// Skip
			return Win32Interop.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
		}

		// Event handlers

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
			if (selectedWindow != null) StartWorker();
			else StopWorker();
		}

		private void ProcessNameInput_TextChanged(object sender, TextChangedEventArgs e)
		{
			var box = e.Source as TextBox;
			searchText = box.Text;
			WindowListViewSource.View.Refresh();
		}

		private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
		{
			var item = e.Item as WindowInformation;
			var titleMatches = item.Name.ToLower().Contains(searchText);
			var procNameMatches = item.ProcessName.ToLower().Contains(searchText);
			e.Accepted = titleMatches || procNameMatches;
		}

		private void TrapMouse_Click(object sender, RoutedEventArgs e)
		{
			WindowModel.MouseTrapRequested = !WindowModel.MouseTrapRequested;
		}
	}
}
