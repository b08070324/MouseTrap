using MouseTrap.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MouseTrap.Foundation
{
	// Notes
	// var foregroundWindow = Win32Interop.GetForegroundWindow();
	// Gets pointer to a window
	// Can use this to see if matches handle or process path
	// IsWindowInForeground(ptr)
	// IsProcessInForeground(string)

	// Therefore selected window can be either a handle or process path
	// Need to handle this somehow

	public class WindowManager
	{
		public DetailedWindowItem SelectedWindow { get; private set; }

		public void SelectWindow(WindowItem windowItem)
		{
			if (windowItem == null) SelectedWindow = null;
			else SelectedWindow = GetWindowDetails(windowItem);
		}

		public bool UpdateWindowDetails()
		{
			return UpdateWindowDetails(SelectedWindow);
		}

		public ICollection<WindowItem> GetWindowList()
		{
			var list = new List<WindowItem>();
			var currentProcessId = Process.GetCurrentProcess().Id;

			Win32Interop.EnumWindows((hWnd, lParam) =>
			{
				// Data - Get process ID for window
				Win32Interop.GetWindowThreadProcessId(hWnd, out uint procId);

				// Filter - Ignore self
				if (procId == currentProcessId) return true;

				// Filter - Check visibility
				if (!Win32Interop.IsWindowVisible(hWnd) || Win32Interop.IsIconic(hWnd)) return true;

				// Filter - Ignore tool windows
				if (Win32Interop.WindowHasExStyle(hWnd, WindowStylesEx.WS_EX_TOOLWINDOW | WindowStylesEx.WS_EX_NOREDIRECTIONBITMAP)) return true;

				// Name of executable
				string processName;
				using (var process = Process.GetProcessById((int)procId))
				{
					processName = process.ProcessName;
				}

				// Data - Get window title
				var title = Win32Interop.GetWindowText(hWnd);

				// Add to list
				list.Add(new WindowItem { Handle = hWnd, ProcessId = procId, ProcessName = processName, Title = title });

				return true;
			}, 0);

			return list;
		}

		private DetailedWindowItem GetWindowDetails(WindowItem windowItem)
		{
			if (windowItem == null) return null;

			// Create details from item
			var detailedItem = new DetailedWindowItem(windowItem);

			// Get process path
			GetFullProcessName(detailedItem);

			// Get remaining details
			UpdateWindowDetails(detailedItem);

			// Return details
			return detailedItem;
		}

		// Gets full path to process exe, expensive to perform
		private void GetFullProcessName(DetailedWindowItem detailedItem)
		{
			// Get limited info handle
			IntPtr limitedHandle = Win32Interop.OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, (int)detailedItem.ProcessId);

			// Full path to executable
			var sb = new StringBuilder(1024);
			uint len = (uint)sb.Capacity + 1;
			if (Win32Interop.QueryFullProcessImageName(limitedHandle, 0, sb, ref len))
			{
				detailedItem.FullProcessName = sb.ToString();
			}

			// Close handle
			Win32Interop.CloseHandle(limitedHandle);
		}

		// Updates Title, BoundingDimensions and IsInForeground
		private bool UpdateWindowDetails(DetailedWindowItem detailedItem)
		{
			// Check handle
			if (!Win32Interop.IsWindow(detailedItem.Handle)) return false;

			// Get window title
			detailedItem.Title = Win32Interop.GetWindowText(detailedItem.Handle);

			// Get window dimensions
			if (Win32Interop.GetWindowRect(detailedItem.Handle, out Rect rect))
			{
				detailedItem.BoundingDimensions = rect;
			}

			// Check if window is in foreground
			var fgHandle = Win32Interop.GetForegroundWindow();
			detailedItem.IsInForeground = (fgHandle == detailedItem.Handle);

			return true;
		}
	}
}
