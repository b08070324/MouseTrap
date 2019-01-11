using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WpfApp2
{
	public static class Win32Interop
	{
		public delegate bool WindowEnumCallback(IntPtr hWnd, int lParam);

		[StructLayout(LayoutKind.Sequential)]
		public struct Rect
		{
			public int Left, Top, Right, Bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Point
		{
			public int X;
			public int Y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct WindowPlacement
		{
			public int Length;
			public int Flags;
			public int ShowCmd;
			public Point MinPosition;
			public Point MaxPosition;
			public Rect NormalPosition;
		}

		public enum GWL : int
		{
			GWL_WNDPROC = (-4),
			GWL_HINSTANCE = (-6),
			GWL_HWNDPARENT = (-8),
			GWL_STYLE = (-16),
			GWL_EXSTYLE = (-20),
			GWL_USERDATA = (-21),
			GWL_ID = (-12)
		}

		[Flags]
		public enum WindowStylesEx : uint
		{
			WS_EX_ACCEPTFILES = 0x00000010,
			WS_EX_APPWINDOW = 0x00040000,
			WS_EX_CLIENTEDGE = 0x00000200,
			WS_EX_COMPOSITED = 0x02000000,
			WS_EX_CONTEXTHELP = 0x00000400,
			WS_EX_CONTROLPARENT = 0x00010000,
			WS_EX_DLGMODALFRAME = 0x00000001,
			WS_EX_LAYERED = 0x00080000,
			WS_EX_LAYOUTRTL = 0x00400000,
			WS_EX_LEFT = 0x00000000,
			WS_EX_LEFTSCROLLBAR = 0x00004000,
			WS_EX_LTRREADING = 0x00000000,
			WS_EX_MDICHILD = 0x00000040,
			WS_EX_NOACTIVATE = 0x08000000,
			WS_EX_NOINHERITLAYOUT = 0x00100000,
			WS_EX_NOPARENTNOTIFY = 0x00000004,
			WS_EX_NOREDIRECTIONBITMAP = 0x00200000,
			WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
			WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
			WS_EX_RIGHT = 0x00001000,
			WS_EX_RIGHTSCROLLBAR = 0x00000000,
			WS_EX_RTLREADING = 0x00002000,
			WS_EX_STATICEDGE = 0x00020000,
			WS_EX_TOOLWINDOW = 0x00000080,
			WS_EX_TOPMOST = 0x00000008,
			WS_EX_TRANSPARENT = 0x00000020,
			WS_EX_WINDOWEDGE = 0x00000100
		}

		public static bool HasExStyle(IntPtr exStyle, WindowStylesEx style)
		{
			return (exStyle.ToInt32() & (int)style) != 0;
		}

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

		public static string GetWindowText(IntPtr hWnd)
		{
			var len = GetWindowTextLength(hWnd);
			StringBuilder sb = new StringBuilder(len + 1);
			GetWindowText(hWnd, sb, sb.Capacity);
			return sb.ToString();
		}
	}
}
