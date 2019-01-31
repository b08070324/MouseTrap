using MouseTrap.Interop;
using System;

namespace MouseTrap.Hooks
{
	public class UpdateHook
	{
		private WinEventHook _windowUpdateHook;
		private WinEventHook _windowClosedHook;
		private IntPtr _targetHandle;

		public event EventHandler<string> WindowTitleChanged;
		public event EventHandler<Win32Rect> WindowRectChanged;
		public event EventHandler WindowClosed;

		public UpdateHook()
		{
			_windowUpdateHook = new WinEventHook();
			_windowUpdateHook.EventHandler += OnWindowUpdateEvent;
			_windowClosedHook = new WinEventHook();
			_windowClosedHook.EventHandler += OnWindowClosedEvent;
		}

		public void StartHook(IntPtr handle)
		{
			if (_targetHandle == IntPtr.Zero)
			{
				_targetHandle = handle;
				_windowUpdateHook.StartHook(WinEventConstant.EVENT_OBJECT_LOCATIONCHANGE, WinEventConstant.EVENT_OBJECT_NAMECHANGE, _targetHandle);
				_windowClosedHook.StartHook(WinEventConstant.EVENT_OBJECT_DESTROY, 0, _targetHandle);
			}
		}

		public void StopHook()
		{
			_windowUpdateHook.StopHook();
			_windowClosedHook.StopHook();
			_targetHandle = IntPtr.Zero;
		}

		private void OnWindowUpdateEvent(object sender, WinEventHookEventArgs e)
		{
			if (e.Handle == _targetHandle && e.ObjectId == 0)
			{
				if (e.EventType == WinEventConstant.EVENT_OBJECT_NAMECHANGE)
				{
					var title = Win32Interop.GetWindowText(_targetHandle);
					WindowTitleChanged?.Invoke(this, title);
				}
				else if (e.EventType == WinEventConstant.EVENT_OBJECT_LOCATIONCHANGE)
				{
					Win32Interop.GetWindowRect(_targetHandle, out Win32Rect rect);
					WindowRectChanged?.Invoke(this, rect);
				}
			}
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
