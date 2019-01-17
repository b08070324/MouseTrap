using MouseTrap.Interop;
using System;

namespace MouseTrap.Foundation
{
	public class MouseHookManager
	{
		public bool TrapMouse { get; set; }

		private readonly HookProc mouseHookCallback;
		private IntPtr mouseHookPtr;
		private Rect margin;
		private Rect region;
		private Rect boundaries;

		public MouseHookManager()
		{
			mouseHookCallback = new HookProc(MouseHookCallbackFunction);
			mouseHookPtr = IntPtr.Zero;
			margin = new Rect { Bottom = -8, Left = 8, Right = -8, Top = 8 };
			region = new Rect();
			boundaries = new Rect();
		}

		public void SetRegion(Rect dimensions)
		{
			UpdateRect(ref region, ref dimensions);
		}

		public void SetMargin(Rect dimensions)
		{
			UpdateRect(ref margin, ref dimensions);
		}

		private void UpdateRect(ref Rect target, ref Rect source)
		{
			target.Bottom = source.Bottom;
			target.Left = source.Left;
			target.Right = source.Right;
			target.Top = source.Top;
			boundaries = region + margin;
		}

		public void HookMouse()
		{
			if (mouseHookPtr == IntPtr.Zero)
			{
				var hMod = System.Runtime.InteropServices.Marshal.GetHINSTANCE(typeof(MainWindow).Module);
				mouseHookPtr = Win32Interop.SetWindowsHookEx(HookType.WH_MOUSE_LL, mouseHookCallback, hMod, 0);
			}
		}

		public void UnhookMouse()
		{
			if (mouseHookPtr != IntPtr.Zero) Win32Interop.UnhookWindowsHookEx(mouseHookPtr);
			mouseHookPtr = IntPtr.Zero;
		}

		private IntPtr MouseHookCallbackFunction(int code, IntPtr wParam, IntPtr lParam)
		{
			// Only handle WM_MOUSEMOVE messages
			var isMouseMove = (wParam.ToInt32() == 0x0200); // #define WM_MOUSEMOVE 0x0200

			// Check if message should be handled
			if (code >= 0 && isMouseMove && TrapMouse)
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
				Win32Interop.SetCursorPos(point.X, point.Y);

				// Done
				return new IntPtr(1);
			}

			// Skip
			return Win32Interop.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
		}
	}
}
