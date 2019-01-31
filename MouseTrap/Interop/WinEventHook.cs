using System;

namespace MouseTrap.Interop
{
	public class WinEventHook : IDisposable
	{
		bool disposed = false;

		// Ensure delegate is not collected prematurely
		private readonly WinEventDelegate _winEventDelegate;
		private IntPtr _winEventHook;
		private WinEventConstant _winEventMin;
		private WinEventConstant _winEventMax;
		private uint _processId;
		private uint _threadId;

		// Event handler
		public event EventHandler<WinEventHookEventArgs> EventHandler;

		// Constructor
		public WinEventHook()
		{
			_winEventHook = IntPtr.Zero;
			_winEventDelegate = new WinEventDelegate(WinEventCallback);
		}

		public void StartHook(WinEventConstant winEventMin, WinEventConstant winEventMax = 0, IntPtr processHandle = default(IntPtr))
		{
			if (_winEventHook == IntPtr.Zero)
			{
				// If the idProcess parameter is nonzero and idThread is zero, the hook function receives 
				// the specified events from all threads in that process. If the idProcess parameter is 
				// zero and idThread is nonzero, the hook function receives the specified events only from 
				// the thread specified by idThread. If both are zero, the hook function receives the 
				// specified events from all threads and processes.
				_processId = 0;
				_threadId = 0;
				if (processHandle != IntPtr.Zero) _threadId = Win32Interop.GetWindowThreadProcessId(processHandle, out _processId);

				_winEventMin = winEventMin;
				_winEventMax = winEventMax > 0 ? winEventMax : winEventMin;

				_winEventHook = Win32Interop.SetWinEventHook(
					(uint)_winEventMin,
					(uint)_winEventMax,
					IntPtr.Zero, _winEventDelegate, _processId, _threadId, (uint)WinEventConstant.WINEVENT_OUTOFCONTEXT);
			}
		}

		public void StopHook()
		{
			if (_winEventHook != IntPtr.Zero && Win32Interop.UnhookWinEvent(_winEventHook))
			{
				_winEventHook = IntPtr.Zero;
			}
		}

		// IDisposable
		~WinEventHook()
		{
			Dispose(false);
		}

		// IDisposable
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// IDisposable
		protected virtual void Dispose(bool disposing)
		{
			if (disposed) return;

			if (disposing)
			{
				// Free any other managed objects here.
			}

			// Free any unmanaged objects here.
			StopHook();

			// Done
			disposed = true;
		}

		// Callback triggered when hook triggers
		private void WinEventCallback(IntPtr hWinEventHook, uint eventType,
			IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
		{
			if (hwnd != IntPtr.Zero)
			{
				EventHandler?.Invoke(this, new WinEventHookEventArgs
				{
					EventType = (WinEventConstant)eventType,
					Handle = hwnd,
					ObjectId = idObject,
					ChildId = idChild
				});
			}
		}
	}
}
