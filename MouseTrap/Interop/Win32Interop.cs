using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MouseTrap.Interop
{
	public delegate bool WindowEnumCallback(IntPtr hWnd, int lParam);

	public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

	public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, 
		IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

	public static class NativeMethods
	{
		[DllImport("user32.dll")]
		internal static extern bool EnumWindows(WindowEnumCallback lpEnumFunc, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

		[DllImport("user32.dll")]
		internal static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("user32.dll")]
		internal static extern bool IsIconic(IntPtr hWnd);

		[DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
		private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
		[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
		private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

		[DllImport("kernel32.dll")]
		internal static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool QueryFullProcessImageName([In] IntPtr hProcess, [In] uint dwFlags, [Out] StringBuilder lpExeName, [In, Out] ref uint lpdwSize);

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool CloseHandle(IntPtr hHandle);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool GetWindowRect(IntPtr hWnd, out Win32Rect lpRect);

		[DllImport("user32.dll")]
		internal static extern bool IsWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		internal static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, 
			IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

		[DllImport("user32.dll")]
		internal static extern bool UnhookWinEvent(IntPtr hWinEventHook);

		[DllImport("user32.dll")]
		internal static extern bool SetCursorPos(int X, int Y);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll")]
		internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

		// This static method is required because Win32 does not support
		// GetWindowLongPtr directly
		internal static IntPtr GetWindowLong(IntPtr hWnd, int nIndex)
		{
			if (IntPtr.Size == 4) return new IntPtr(GetWindowLong32(hWnd, nIndex));
			return GetWindowLongPtr64(hWnd, nIndex);
		}

		internal static bool WindowHasStyle(IntPtr hWnd, WindowStyles requiredStyle)
		{
			var style = NativeMethods.GetWindowLong(hWnd, (int)GWL.GWL_STYLE);
			return (style.ToInt32() & (int)requiredStyle) != 0;
		}

		internal static bool WindowHasExStyle(IntPtr hWnd, WindowStylesEx requiredStyle)
		{
			var exStyle = NativeMethods.GetWindowLong(hWnd, (int)GWL.GWL_EXSTYLE);
			return (exStyle.ToInt32() & (int)requiredStyle) != 0;
		}

		internal static void GetFullProcessName(StringBuilder sb, int processId)
		{
			sb.Clear();
			uint len = (uint)sb.Capacity + 1;
			IntPtr limitedHandle = IntPtr.Zero;

			try
			{
				limitedHandle = NativeMethods.OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, processId);
				NativeMethods.QueryFullProcessImageName(limitedHandle, 0, sb, ref len);
			}
			catch (Exception)
			{
			}
			finally
			{
				if (limitedHandle != IntPtr.Zero) NativeMethods.CloseHandle(limitedHandle);
			}
		}

		internal static string GetWindowText(IntPtr hWnd)
		{
			var len = GetWindowTextLength(hWnd);
			StringBuilder sb = new StringBuilder(len + 1);
			GetWindowText(hWnd, sb, sb.Capacity);
			return sb.ToString();
		}
	}
}
