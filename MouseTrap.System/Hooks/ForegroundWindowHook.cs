using MouseTrap.Interop;
using System;

namespace MouseTrap.Hooks
{
	internal interface IForegroundWindowHook
	{
		void StartHook();
		void StopHook();
		event EventHandler<ForegroundWindowChangedEventArgs> ForegroundWindowChanged;
	}

	internal sealed class ForegroundWindowHook : WinEventHook, IForegroundWindowHook
	{
		private uint LastProcessId { get; set; }
		public event EventHandler<ForegroundWindowChangedEventArgs> ForegroundWindowChanged;

		public void StartHook()
		{
			if (!HasEventHookInstance)
			{
				StartWinEventHook(WinEventConstant.EVENT_SYSTEM_FOREGROUND);
			}
		}

		public void StopHook()
		{
			StopWinEventHook();
		}

		protected override void WinEventCallback(WinEventConstant eventType, IntPtr handle, int objectId, int childId)
		{
			if (handle != null && objectId == 0)
			{
				// Ignore these windows
				if (NativeMethods.WindowHasExStyle(handle, WindowStylesEx.WS_EX_NOACTIVATE)) return;

				// Get process ID and check against last
				NativeMethods.GetWindowThreadProcessId(handle, out uint windowThreadProcId);

				if (windowThreadProcId != LastProcessId)
				{
					LastProcessId = windowThreadProcId;
					string processName = NativeMethods.GetFullProcessName((int)windowThreadProcId);

					ForegroundWindowChanged?.Invoke(this, new ForegroundWindowChangedEventArgs
					{
						Handle = handle,
						WindowThreadProcId = windowThreadProcId,
						ProcessPath = processName
					});
				}
			}
		}
	}
}