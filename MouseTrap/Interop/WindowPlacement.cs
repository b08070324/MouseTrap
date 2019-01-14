using System.Runtime.InteropServices;

namespace MouseTrap.Interop
{
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
}
