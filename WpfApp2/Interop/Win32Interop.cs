using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WpfApp2.Interop
{
	public delegate bool WindowEnumCallback(IntPtr hWnd, int lParam);

	public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

	public static class Win32Interop
	{
		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

		[DllImport("user32.dll")]
		public static extern bool EnumWindows(WindowEnumCallback lpEnumFunc, int lParam);

		[DllImport("user32.dll")]
		public static extern bool IsWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern bool IsIconic(IntPtr hWnd);

		[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
		private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
		private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

		// This static method is required because Win32 does not support
		// GetWindowLongPtr directly
		public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
		{
			if (IntPtr.Size == 8) return GetWindowLongPtr64(hWnd, nIndex);
			else return GetWindowLongPtr32(hWnd, nIndex);
		}

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement windowPlacement);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hHandle);

		[DllImport("Kernel32.dll")]
		public static extern bool QueryFullProcessImageName([In] IntPtr hProcess, [In] uint dwFlags, [Out] StringBuilder lpExeName, [In, Out] ref uint lpdwSize);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll")]
		public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll")]
		public static extern bool SetCursorPos(int X, int Y);

		public static bool HasExStyle(IntPtr exStyle, WindowStylesEx style)
		{
			return (exStyle.ToInt32() & (int)style) != 0;
		}

		public static string GetWindowText(IntPtr hWnd)
		{
			var len = GetWindowTextLength(hWnd);
			StringBuilder sb = new StringBuilder(len + 1);
			GetWindowText(hWnd, sb, sb.Capacity);
			return sb.ToString();
		}
	}
}
