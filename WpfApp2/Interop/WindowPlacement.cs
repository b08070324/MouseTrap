using System.Runtime.InteropServices;

namespace WpfApp2.Interop
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
