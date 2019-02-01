using MouseTrap.Interop;
using System;

namespace MouseTrap.Hooks
{
	public class WindowUpdateHook : WinEventHook
	{
		private IntPtr _targetHandle;
		public event EventHandler<WindowTitleEventArgs> WindowTitleChanged;
		public event EventHandler<WindowRectEventArgs> WindowRectChanged;

		public WindowUpdateHook()
		{
			WinEventHookEvent += OnWindowUpdateEvent;
		}

		public void StartHook(IntPtr handle)
		{
			if (_targetHandle == IntPtr.Zero)
			{
				_targetHandle = handle;
				StartWinEventHook(WinEventConstant.EVENT_OBJECT_LOCATIONCHANGE, WinEventConstant.EVENT_OBJECT_NAMECHANGE, _targetHandle);
			}
		}

		public void StopHook()
		{
			StopWinEventHook();
			_targetHandle = IntPtr.Zero;
		}

		private void OnWindowUpdateEvent(object sender, WinEventHookEventArgs e)
		{
			if (e.Handle == _targetHandle && e.ObjectId == 0)
			{
				if (e.EventType == WinEventConstant.EVENT_OBJECT_NAMECHANGE)
				{
					var title = NativeMethods.GetWindowText(_targetHandle);
					WindowTitleChanged?.Invoke(this, e: new WindowTitleEventArgs { Title = title });
				}
				else if (e.EventType == WinEventConstant.EVENT_OBJECT_LOCATIONCHANGE)
				{
					NativeMethods.GetWindowRect(_targetHandle, out Win32Rect rect);
					WindowRectChanged?.Invoke(this, e: new WindowRectEventArgs { WindowRect = rect });
				}
			}
		}
	}
}
