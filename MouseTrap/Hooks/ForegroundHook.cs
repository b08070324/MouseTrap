using MouseTrap.Interop;
using System;
using System.Text;

namespace MouseTrap.Hooks
{
	public class ForegroundHook : WinEventHook
	{
		private StringBuilder _sb = new StringBuilder(1024);
		private uint lastProcessId;
		public event EventHandler<ForegroundHookEventArgs> ForegroundWindowChanged;

		public ForegroundHook() : base()
		{
			WinEventHookEvent += OnWinHookEvent;
		}

		public void StartHook()
		{
			StartWinEventHook(WinEventConstant.EVENT_SYSTEM_FOREGROUND);
		}

		public void StopHook()
		{
			StopWinEventHook();
		}

		private void OnWinHookEvent(object sender, WinEventHookEventArgs e)
		{
			if (e.Handle != null && e.ObjectId == 0)
			{
				// Ignore these windows
				if (NativeMethods.WindowHasExStyle(e.Handle, WindowStylesEx.WS_EX_NOACTIVATE)) return;

				// Get process ID and check against last
				NativeMethods.GetWindowThreadProcessId(e.Handle, out uint windowThreadProcId);

				if (windowThreadProcId != lastProcessId)
				{
					lastProcessId = windowThreadProcId;
					NativeMethods.GetFullProcessName(_sb, (int)windowThreadProcId);
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
