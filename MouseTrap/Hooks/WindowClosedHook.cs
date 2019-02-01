using MouseTrap.Interop;
using System;

namespace MouseTrap.Hooks
{
	public class WindowClosedHook : WinEventHook
	{
		private IntPtr _targetHandle;
		public event EventHandler WindowClosed;

		public WindowClosedHook() : base()
		{
			WinEventHookEvent += OnWindowClosedEvent;
		}

		public void StartHook(IntPtr handle)
		{
			if (_targetHandle == IntPtr.Zero)
			{
				_targetHandle = handle;
				StartWinEventHook(WinEventConstant.EVENT_OBJECT_DESTROY, 0, _targetHandle);
			}
		}

		public void StopHook()
		{
			StopWinEventHook();
			_targetHandle = IntPtr.Zero;
		}

		private void OnWindowClosedEvent(object sender, WinEventHookEventArgs e)
		{
			if (e.EventType == WinEventConstant.EVENT_OBJECT_DESTROY && e.Handle == _targetHandle && e.ObjectId == 0)
			{
				WindowClosed?.Invoke(this, null);
			}
		}
	}
}
