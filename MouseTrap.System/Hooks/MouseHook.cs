using MouseTrap.Interop;
using MouseTrap.Models;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static System.Diagnostics.Debug;

namespace MouseTrap.Hooks
{
	internal interface IMouseHook : IDisposable
	{
		void StartHook(Dimensions region);
		void StopHook();
		void SetRegion(Dimensions region);
		void SetState(bool isRestricted);
	}

	internal class MouseHook : IMouseHook
	{
		private IntPtr _mouseHookPtr;

		private bool IsDisposed { get; set; } = false;
		private HookProc MouseHookCallback { get; }
		private IntPtr MouseHookPtr { get => _mouseHookPtr; set => _mouseHookPtr = value; }
		private Dimensions Boundaries { get; }
		private bool IsRestricted { get; set; }

		public MouseHook()
		{
			MouseHookCallback = new HookProc(MouseHookCallbackFunction);
			MouseHookPtr = IntPtr.Zero;
			Boundaries = new Dimensions();
		}

		public void StartHook(Dimensions region)
		{
			if (MouseHookPtr == IntPtr.Zero)
			{
				// Set region
				SetRegion(region);

				// Set hook, using field to avoid issues with GetLastWin32Error
				_mouseHookPtr = NativeMethods.SetWindowsHookEx(HookType.WH_MOUSE_LL, MouseHookCallback, IntPtr.Zero, 0);
				var errorCode = Marshal.GetLastWin32Error();

				// TODO improve error handling
				if (MouseHookPtr == IntPtr.Zero)
				{
					string errorMessage = new Win32Exception(errorCode).Message;
					WriteLine($"{errorCode} {errorMessage}");
				}
			}
		}

		public void StopHook()
		{
			if (MouseHookPtr != IntPtr.Zero) NativeMethods.UnhookWindowsHookEx(MouseHookPtr);
			MouseHookPtr = IntPtr.Zero;
		}

		public void SetRegion(Dimensions region)
		{
			Boundaries.Left = region.Left;
			Boundaries.Top = region.Top;
			Boundaries.Right = region.Right;
			Boundaries.Bottom = region.Bottom;
		}

		public void SetState(bool isRestricted)
		{
			IsRestricted = isRestricted;
		}

		private bool BoundaryIsValid
		{
			get
			{
				var width = Boundaries.Right - Boundaries.Left;
				var height = Boundaries.Bottom - Boundaries.Top;
				return (width > 0) && (height > 0);
			}
		}

		private IntPtr MouseHookCallbackFunction(int code, IntPtr wParam, IntPtr lParam)
		{
			// Only handle WM_MOUSEMOVE messages
			var isMouseMoveMsg = (wParam.ToInt32() == 0x0200); // #define WM_MOUSEMOVE 0x0200

			// Check if message should be handled
			if (IsRestricted && isMouseMoveMsg && code >= 0 && BoundaryIsValid)
			{
				// Get pointer data
				var mouseInfo = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
				var point = mouseInfo.pt;

				// Limit X
				if (point.X < Boundaries.Left) point.X = (int)Boundaries.Left;
				else if (point.X > Boundaries.Right) point.X = (int)Boundaries.Right;

				// Limit Y
				if (point.Y < Boundaries.Top) point.Y = (int)Boundaries.Top;
				else if (point.Y > Boundaries.Bottom) point.Y = (int)Boundaries.Bottom;

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
			if (IsDisposed) return;

			if (disposing)
			{
				// Free any other managed objects here.
			}

			// Free any unmanaged objects here.
			StopHook();

			// Done
			IsDisposed = true;
		}
	}
}
