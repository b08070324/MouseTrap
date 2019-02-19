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
			// Filter for visibility, tool windows
			if (NativeMethods.IsWindowVisible(hWnd) && 
				!NativeMethods.IsIconic(hWnd) && 
				!NativeMethods.WindowHasExStyle(hWnd, WindowStylesEx.WS_EX_TOOLWINDOW | WindowStylesEx.WS_EX_NOREDIRECTIONBITMAP))
			{
				// Get window details
				var window = new EnumeratedWindow(hWnd);

				// Filter for windows in same process
				if (window.ProcessId != CurrentProcessId)
				{
					// Send window details to client
					Callback(window);
				}
			}

			// Continue iterating
			return true;
		}
	}
}
