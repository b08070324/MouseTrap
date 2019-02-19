using System;

namespace MouseTrap.Interop
{
	internal abstract class WinEventHook : IDisposable
	{
		private uint _processId;

		// Ensure delegate is not collected prematurely
		private WinEventDelegate WinEventDelegate { get; }
		private IntPtr EventHookInstance { get; set; }
		private WinEventConstant WinEventMin { get; set; }
		private WinEventConstant WinEventMax { get; set; }
		private uint ProcessId { get => _processId; set => _processId = value; }
		private uint ThreadId { get; set; }

		protected bool HasEventHookInstance { get => (EventHookInstance != IntPtr.Zero); }

		// Constructor
		public WinEventHook()
		{
			EventHookInstance = IntPtr.Zero;
			WinEventDelegate = new WinEventDelegate(WinEventCallback);
		}

		protected void StartWinEventHook(WinEventConstant winEventMin, WinEventConstant winEventMax = 0, IntPtr processHandle = default)
		{
			if (!HasEventHookInstance)
			{
				// If the idProcess parameter is nonzero and idThread is zero, the hook function receives 
				// the specified events from all threads in that process. If the idProcess parameter is 
				// zero and idThread is nonzero, the hook function receives the specified events only from 
				// the thread specified by idThread. If both are zero, the hook function receives the 
				// specified events from all threads and processes.
				ProcessId = 0;
				ThreadId = 0;
				if (processHandle != IntPtr.Zero) ThreadId = NativeMethods.GetWindowThreadProcessId(processHandle, out _processId);

				WinEventMin = winEventMin;
				WinEventMax = winEventMax > 0 ? winEventMax : winEventMin;

				EventHookInstance = NativeMethods.SetWinEventHook(
					(uint)WinEventMin,
					(uint)WinEventMax,
					IntPtr.Zero, WinEventDelegate, ProcessId, ThreadId, (uint)WinEventConstant.WINEVENT_OUTOFCONTEXT);
			}
		}

		protected void StopWinEventHook()
		{
			if (HasEventHookInstance)
			{
				NativeMethods.UnhookWinEvent(EventHookInstance);
				EventHookInstance = IntPtr.Zero;
			}
		}

		// Callback triggered when hook triggers
		private void WinEventCallback(IntPtr hWinEventHook, uint eventType,
			IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
		{
			if (hwnd != IntPtr.Zero)
			{
				WinEventCallback((WinEventConstant)eventType, hwnd, idObject, idChild);
			}
		}

		// Callback implemented by child class
		protected abstract void WinEventCallback(WinEventConstant eventType, IntPtr handle, int objectId, int childId);

		// IDisposable
		bool disposed = false;

		~WinEventHook()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed) return;

			if (disposing)
			{
				// Free any other managed objects here.
			}

			// Free any unmanaged objects here.
			StopWinEventHook();

			// Done
			disposed = true;
		}
	}
}
