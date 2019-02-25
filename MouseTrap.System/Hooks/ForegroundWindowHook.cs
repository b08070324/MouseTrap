using MouseTrap.Interop;
using System;

namespace MouseTrap.Hooks
{
	/// <summary>
	/// Interface for a system hook that raises an event when the foreground window changes
	/// </summary>
	internal interface IForegroundWindowHook
	{
		void StartHook();
		void StopHook();
		event EventHandler<ForegroundWindowChangedEventArgs> ForegroundWindowChanged;
	}

	/// <summary>
	/// Implementation of <see cref="IForegroundWindowHook"/>
	/// </summary>
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
				var windowStyle = NativeMethods.GetWindowStyleEx(handle);
				if ((windowStyle & WindowStylesEx.WS_EX_NOACTIVATE) == WindowStylesEx.WS_EX_NOACTIVATE) return;

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