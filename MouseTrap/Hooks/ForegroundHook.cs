using MouseTrap.Interop;
using System;
using System.Text;

namespace MouseTrap.Hooks
{
	public class ForegroundHook
	{
		private WinEventHook _winHook;
		private StringBuilder _sb = new StringBuilder(1024);
		private uint lastProcessId;

		public event EventHandler<ForegroundHookEventArgs> ForegroundWindowChanged;

		public ForegroundHook()
		{
			_winHook = new WinEventHook();
			_winHook.EventHandler += OnWinHookEvent;
		}

		public void StartHook()
		{
			_winHook.StartHook(WinEventConstant.EVENT_SYSTEM_FOREGROUND);
		}

		public void StopHook()
		{
			_winHook.StopHook();
		}

		private void OnWinHookEvent(object sender, WinEventHookEventArgs e)
		{
			if (e.Handle != null && e.ObjectId == 0)
			{
				// Ignore these windows
				if (Win32Interop.WindowHasExStyle(e.Handle, WindowStylesEx.WS_EX_NOACTIVATE)) return;

				// Get process ID and check against last
				Win32Interop.GetWindowThreadProcessId(e.Handle, out uint windowThreadProcId);

				if (windowThreadProcId != lastProcessId)
				{
					lastProcessId = windowThreadProcId;
					Win32Interop.GetFullProcessName(_sb, (int)windowThreadProcId);
					string processName = _sb.ToString();

					ForegroundWindowChanged?.Invoke(this, new ForegroundHookEventArgs
					{
						Handle = e.Handle,
						WindowThreadProcId = windowThreadProcId,
						ProcessPath = processName
					});
				}
			}
		}
	}
}
