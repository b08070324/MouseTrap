using MouseTrap.Interop;
using MouseTrap.Models;
using System;
using System.Diagnostics;

namespace MouseTrap.Data
{
	public class WindowCatalogue : IWindowCatalogue
	{
		private int CurrentProcessId { get; }
		private Action<IWindow> Callback { get; set; }

		private static readonly string[] BlacklistedClassNames = new string[]
		{
			"Shell_TrayWnd",
			"Shell_SecondaryTrayWnd",
			"SysListView32",
			"Progman"
		};

		public WindowCatalogue()
		{
			CurrentProcessId = Process.GetCurrentProcess().Id;
		}

		public void EnumerateWindows(Action<IWindow> callback)
		{
			Callback = callback;
			NativeMethods.EnumWindows(InteropCallback, IntPtr.Zero);
		}

		private bool InteropCallback(IntPtr hWnd, int lParam)
		{
			// Filter for visibility
			if (!NativeMethods.IsWindowVisible(hWnd)) return true;

			// Filter for tool windows
			if (NativeMethods.WindowHasExStyle(hWnd, WindowStylesEx.WS_EX_TOOLWINDOW)) return true;

			// Get window details
			var window = new EnumeratedWindow(hWnd);

			// Filter for windows in same process
			if (window.ProcessId == CurrentProcessId) return true;

			// Filter title length
			if (string.IsNullOrEmpty(window.Title)) return true;

			// Send window details to client
			Callback(window);

			// Continue iterating
			return true;
		}
	}
}
