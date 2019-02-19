using MouseTrap.Interop;
using MouseTrap.Models;
using System;

namespace MouseTrap.Hooks
{
	internal interface IWindowUpdateHook
	{
		void StartHook(IntPtr handle);
		void StopHook();
		event EventHandler WindowClosed;
		event EventHandler<TitleChangedEventArgs> TitleChanged;
		event EventHandler<DimensionsChangedEventArgs> DimensionsChanged;
	}

	internal sealed class WindowUpdateHook : WinEventHook, IWindowUpdateHook
	{
		private IntPtr TargetHandle { get; set; } = IntPtr.Zero;

		public void StartHook(IntPtr handle)
		{
			if (handle != IntPtr.Zero && TargetHandle == IntPtr.Zero)
			{
				TargetHandle = handle;
				StartWinEventHook(WinEventConstant.EVENT_OBJECT_DESTROY, WinEventConstant.EVENT_OBJECT_NAMECHANGE, handle);
			}
		}

		public void StopHook()
		{
			StopWinEventHook();
			TargetHandle = IntPtr.Zero;
		}

		protected override void WinEventCallback(WinEventConstant eventType, IntPtr handle, int objectId, int childId)
		{
			if (handle == TargetHandle && objectId == 0)
			{
				if (eventType == WinEventConstant.EVENT_OBJECT_DESTROY)
				{
					// Target window was closed
					WindowClosed?.Invoke(this, EventArgs.Empty);
				}
				else if (eventType == WinEventConstant.EVENT_OBJECT_LOCATIONCHANGE)
				{
					// Target window size has changed
					NativeMethods.GetWindowRect(handle, out Win32Rect rect);
					DimensionsChanged?.Invoke(this, new DimensionsChangedEventArgs
					{
						Dimensions = new Dimensions
						{
							Left = rect.Left,
							Top = rect.Top,
							Right = rect.Right,
							Bottom = rect.Bottom
						}
					});
				}
				else if (eventType == WinEventConstant.EVENT_OBJECT_NAMECHANGE)
				{
					// Target window title has changed
					var title = NativeMethods.GetWindowText(handle);
					TitleChanged?.Invoke(this, new TitleChangedEventArgs { Title = title });
				}
			}
		}

		public event EventHandler WindowClosed;
		public event EventHandler<TitleChangedEventArgs> TitleChanged;
		public event EventHandler<DimensionsChangedEventArgs> DimensionsChanged;
	}
}
