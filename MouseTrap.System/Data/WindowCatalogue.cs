using MouseTrap.Interop;
using MouseTrap.Models;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MouseTrap.Data
{
	public class WindowCatalogue : IWindowCatalogue
	{
		private bool ShouldIncludeWindow(IntPtr handle, uint processId, string title)
		{
			// Filter for visibility
			if (!NativeMethods.IsWindowVisible(handle)) return false;

			// Filter elements with no name
			if (string.IsNullOrEmpty(title)) return false;

			// Filter for windows in same process
			var mouseTrapProcessId = Process.GetCurrentProcess().Id;
			if (processId == mouseTrapProcessId) return false;

			// Filter for app windows
			var windowStylesEx = NativeMethods.GetWindowStyleEx(handle);
			var windowStylesExFilter = WindowStylesEx.WS_EX_APPWINDOW;
			if ((windowStylesEx & windowStylesExFilter) != 0) return true;

			// Filter for no activate, tool windows
			windowStylesExFilter = WindowStylesEx.WS_EX_TOOLWINDOW | WindowStylesEx.WS_EX_NOACTIVATE;
			if ((windowStylesEx & windowStylesExFilter) != 0) return false;

			// Ignore core windows
			var className = NativeMethods.GetClassName(handle);
			if (className == "Windows.UI.Core.CoreWindow") return false;

			// Ignore windows that are cloaked
			if (NativeMethods.IsWindowCloaked(handle)) return false;

			// Ignore WPF windows that are inactive
			// This might not be required, keeping the code here atm
			//bool windowInactive = false;
			//if (className == "ApplicationFrameWindow")
			//{
			//	NativeMethods.EnumPropsEx(handle, (hwnd, lpszString, hData, dwData) =>
			//	{
			//		// Get property name as string
			//		string propName = Marshal.PtrToStringAnsi(lpszString);

			//		// Check property value
			//		if (propName == "ApplicationViewCloakType")
			//		{
			//			// 1 is inactive
			//			windowInactive = (dwData.ToInt32() == 1);

			//			// Exit callback loop
			//			return 0;
			//		}

			//		// Continue callback loop
			//		return 1;
			//	}, IntPtr.Zero);
			//}
			//if (windowInactive) return false;

			return true;
		}

		public void EnumerateWindows(Action<IWindow> callback)
		{
			NativeMethods.EnumWindows((hWnd, lParam) =>
			{
				// Get process ID
				NativeMethods.GetWindowThreadProcessId(hWnd, out uint processId);

				// Get title
				var title = NativeMethods.GetWindowText(hWnd);

				// Validate window for list inclusion
				if (ShouldIncludeWindow(hWnd, processId, title))
				{
					// Get dimensions
					NativeMethods.GetWindowRect(hWnd, out Win32Rect rect);

					// Send window details to callback
					callback(new EnumeratedWindow
					{
						Handle = hWnd,
						ProcessId = processId,
						ProcessPath = NativeMethods.GetFullProcessName((int)processId),
						Title = title,
						Left = rect.Left,
						Top = rect.Top,
						Right = rect.Right,
						Bottom = rect.Bottom,
						IsMinimized = NativeMethods.IsIconic(hWnd)
					});
				}

				// Continue iterating
				return true;
			}, IntPtr.Zero);
		}
	}
}
