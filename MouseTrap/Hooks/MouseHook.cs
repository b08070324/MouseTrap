using MouseTrap.Interop;
using System;

namespace MouseTrap.Hooks
{
	public class MouseHook : IDisposable
	{
		private bool disposed = false;
		private readonly HookProc mouseHookCallback;
		private IntPtr mouseHookPtr;
		private Win32Rect margin;
		private Win32Rect region;
		private Win32Rect boundaries;

		public MouseHook()
		{
			mouseHookCallback = new HookProc(MouseHookCallbackFunction);
			mouseHookPtr = IntPtr.Zero;
			margin = new Win32Rect { Bottom = 0, Left = 0, Right = 0, Top = 0 };
			region = new Win32Rect();
			boundaries = new Win32Rect();
		}

		public bool TrapMouse { get; set; }

		public void SetRegion(Win32Rect dimensions)
		{
			UpdateRect(ref region, ref dimensions);
		}

		public void SetMargin(Win32Rect dimensions)
		{
			UpdateRect(ref margin, ref dimensions);
		}

		private void UpdateRect(ref Win32Rect target, ref Win32Rect source)
		{
			target.Bottom = source.Bottom;
			target.Left = source.Left;
			target.Right = source.Right;
			target.Top = source.Top;
			boundaries = region + margin;
			return;
		}

		public void StartHook()
		{
			if (mouseHookPtr == IntPtr.Zero)
			{
				var hMod = System.Runtime.InteropServices.Marshal.GetHINSTANCE(typeof(MainWindow).Module);
				mouseHookPtr = NativeMethods.SetWindowsHookEx(HookType.WH_MOUSE_LL, mouseHookCallback, hMod, 0);
			}
		}

		public void StopHook()
		{
			if (mouseHookPtr != IntPtr.Zero) NativeMethods.UnhookWindowsHookEx(mouseHookPtr);
			mouseHookPtr = IntPtr.Zero;
		}

		private bool BoundaryIsValid
		{
			get
			{
				var width = boundaries.Right - boundaries.Left;
				var height = boundaries.Bottom - boundaries.Top;
				return (width > 0) && (height > 0);
			}
		}

		private IntPtr MouseHookCallbackFunction(int code, IntPtr wParam, IntPtr lParam)
		{
			// Only handle WM_MOUSEMOVE messages
			var isMouseMove = (wParam.ToInt32() == 0x0200); // #define WM_MOUSEMOVE 0x0200

			// Check if message should be handled
			if (code >= 0 && isMouseMove && TrapMouse && BoundaryIsValid)
			{
				// Get pointer data
				var mouseInfo = (MSLLHOOKSTRUCT)System.Runtime.InteropServices.Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
				var point = mouseInfo.pt;

				// Limit X
				if (point.X < boundaries.Left) point.X = boundaries.Left;
				else if (point.X > boundaries.Right) point.X = boundaries.Right;

				// Limit Y
				if (point.Y < boundaries.Top) point.Y = boundaries.Top;
				else if (point.Y > boundaries.Bottom) point.Y = boundaries.Bottom;

				// Move cursor
				NativeMethods.SetCursorPos(point.X, point.Y);

				// Done
				return new IntPtr(1);
			}

			// Skip
			return NativeMethods.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
		}

		// IDisposable
		~MouseHook()
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
	}
}
