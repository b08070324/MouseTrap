using System.Runtime.InteropServices;

namespace MouseTrap.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Rect
	{
		public int Left, Top, Right, Bottom;

		public static Rect operator +(Rect a, Rect b)
		{
			return new Rect
			{
				Bottom = a.Bottom + b.Bottom,
				Left = a.Left + b.Left,
				Right = a.Right + b.Right,
				Top = a.Top + b.Top
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1} {2} {3}", Left, Top, Right, Bottom);
		}
	}
}
