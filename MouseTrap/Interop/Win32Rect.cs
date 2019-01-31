using System.Runtime.InteropServices;

namespace MouseTrap.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Win32Rect
	{
		public int Left, Top, Right, Bottom;

		public static Win32Rect operator +(Win32Rect a, Win32Rect b)
		{
			return new Win32Rect
			{
				Bottom = a.Bottom + b.Bottom,
				Left = a.Left + b.Left,
				Right = a.Right + b.Right,
				Top = a.Top + b.Top
			};
		}

		public override string ToString()
		{
			return $"{Left}, {Top}, {Right}, {Bottom}";
		}
	}
}
