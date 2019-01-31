using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using MouseTrap.Interop;
using MouseTrap.Models;

namespace MouseTrap.WindowQuery
{
	public class WindowQueryManager : IWindowQueryManager
	{
		private readonly int _currentProcessId;

		public WindowQueryManager()
		{
			_currentProcessId = Process.GetCurrentProcess().Id;
		}

		public bool CheckWindow(IWindowItem windowItem)
		{
			return Win32Interop.IsWindow(windowItem.Handle);
		}

		public WindowItemUpdateDetails GetWindowItemUpdate(IWindowItem windowItem)
		{
			StringBuilder sb = new StringBuilder(1024);
			var item = GetWindowItem(sb, windowItem.Handle, windowItem.ProcessId);
			return new WindowItemUpdateDetails { Title = item.Title, Dimensions = item.Dimensions };
		}

		public ObservableCollection<IWindowItem> GetWindowList()
		{
			var list = new ObservableCollection<IWindowItem>();

			StringBuilder sb = new StringBuilder(1024);

			Win32Interop.EnumWindows((hWnd, lParam) =>
			{
				// Data - Get process ID for window
				Win32Interop.GetWindowThreadProcessId(hWnd, out uint windowThreadProcId);

				// Check window
				if (ShouldFilterWindow(hWnd, windowThreadProcId)) return true;

				// Add to list
				list.Add(GetWindowItem(sb, hWnd, windowThreadProcId));

				return true;
			}, 0);

			return list;
		}

		private bool ShouldFilterWindow(IntPtr hWnd, uint windowThreadProcId)
		{
			// Filter - Ignore self
			if (windowThreadProcId == _currentProcessId) return true;

			// Filter - Check visibility
			if (!Win32Interop.IsWindowVisible(hWnd) || Win32Interop.IsIconic(hWnd)) return true;

			// Filter - Ignore tool windows
			if (Win32Interop.WindowHasExStyle(hWnd, WindowStylesEx.WS_EX_TOOLWINDOW | WindowStylesEx.WS_EX_NOREDIRECTIONBITMAP)) return true;

			// Don't filter window
			return false;
		}

		private WindowItem GetWindowItem(StringBuilder sb, IntPtr hWnd, uint windowThreadProcId)
		{
			// Name of executable
			Win32Interop.GetFullProcessName(sb, (int)windowThreadProcId);
			string processName = sb.ToString();

			// Data - Get window title
			var title = Win32Interop.GetWindowText(hWnd);

			// Get window dimensions
			Win32Interop.GetWindowRect(hWnd, out Win32Rect rect);

			// Add to list
			return new WindowItem
			{
				Handle = hWnd,
				ProcessId = windowThreadProcId,
				ProcessPath = processName,
				Title = title,
				Dimensions = new Dimensions { Left = rect.Left, Top = rect.Top, Right = rect.Right, Bottom = rect.Bottom }
			};
		}
	}
}
