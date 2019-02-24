using MouseTrap.Foundation;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MouseTrap.Interop
{
	internal delegate bool WindowEnumCallback(IntPtr hWnd, int lParam);
	internal delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
	internal delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
	internal delegate int EnumPropsExDelegate(IntPtr hwnd, IntPtr lpszString, IntPtr hData, IntPtr dwData);

	internal static class NativeMethods
	{
		private static readonly ObjectPool<StringBuilder> StringBuilderPool;

		static NativeMethods()
		{
			StringBuilderPool = new ObjectPool<StringBuilder>(() => new StringBuilder(1024));
		}

		[DllImport("user32.dll")]
		internal static extern bool EnumWindows(WindowEnumCallback lpEnumFunc, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool GetWindowRect(IntPtr hWnd, out Win32Rect lpRect);

		[DllImport("user32.dll")]
		public static extern bool IsWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		internal static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("user32.dll")]
		internal static extern bool IsIconic(IntPtr hWnd);

		[DllImport("user32.dll")]
		internal static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax,
			IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

		[DllImport("user32.dll")]
		internal static extern bool UnhookWinEvent(IntPtr hWinEventHook);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll")]
		internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		internal static extern bool SetCursorPos(int X, int Y);

		[DllImport("kernel32.dll")]
		internal static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool QueryFullProcessImageName([In] IntPtr hProcess, [In] uint dwFlags, [Out] StringBuilder lpExeName, [In, Out] ref uint lpdwSize);

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool CloseHandle(IntPtr hHandle);

		internal static string GetFullProcessName(int processId)
		{
			// Initialise method result
			string result = string.Empty;

			// Get a stringbuilder from the pool
			var sb = StringBuilderPool.GetObject();
			sb.Clear();

			// Handle for process query
			IntPtr limitedHandle = IntPtr.Zero;

			// Size of buffer for process executable name
			uint len = (uint)sb.Capacity + 1;

			try
			{
				// Open process with limited permissions
				limitedHandle = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, processId);
				
				// Get process executable name
				QueryFullProcessImageName(limitedHandle, 0, sb, ref len);

				// Store result
				result = sb.ToString();
			}
			catch (Exception)
			{
			}
			finally
			{
				// Close handle
				if (limitedHandle != IntPtr.Zero) CloseHandle(limitedHandle);
			}

			// Return to pool
			StringBuilderPool.PutObject(sb);

			// Return result
			return result;
		}

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		internal static string GetWindowText(IntPtr hWnd)
		{
			// Get a stringbuilder from the pool
			var sb = StringBuilderPool.GetObject();
			sb.Clear();

			// Get window text
			GetWindowText(hWnd, sb, sb.Capacity);
			var result = sb.ToString();

			// Return to pool
			StringBuilderPool.PutObject(sb);

			// Return result
			return result;
		}

		[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
		private static extern int GetWindowLongPtr32(IntPtr hWnd, int nIndex);

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
		[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
		private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

		// This static method is required because Win32 does not support
		// GetWindowLongPtr directly
		internal static IntPtr GetWindowLong(IntPtr hWnd, int nIndex)
		{
			if (IntPtr.Size == 4) return new IntPtr(GetWindowLongPtr32(hWnd, nIndex));
			return GetWindowLongPtr64(hWnd, nIndex);
		}

		internal static WindowStylesEx GetWindowStyleEx(IntPtr hWnd)
		{
			return (WindowStylesEx)GetWindowLong(hWnd, (int)GWL.GWL_EXSTYLE).ToInt32();
		}

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

		internal static string GetClassName(IntPtr hWnd)
		{
			// Get a stringbuilder from the pool
			var sb = StringBuilderPool.GetObject();
			sb.Clear();

			// Get class name
			GetClassName(hWnd, sb, sb.Capacity);
			var result = sb.ToString();

			// Return to pool
			StringBuilderPool.PutObject(sb);

			// Return result
			return result;
		}

		[DllImport("user32.dll")]
		internal static extern int EnumPropsEx(IntPtr hWnd, EnumPropsExDelegate lpEnumFunc, IntPtr lParam);

		[DllImport("dwmapi.dll")]
		internal static extern int DwmGetWindowAttribute(IntPtr hWnd, DWMWINDOWATTRIBUTE dwAttribute, out int pvAttribute, int cbAttribute);

		// Check if window is cloaked - this returns false if the call fails
		internal static bool IsWindowCloaked(IntPtr hWnd)
		{
			var result = DwmGetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.Cloaked, out int isCloaked, sizeof(int));
			return (result == 0 && isCloaked != 0);
		}
	}
}
