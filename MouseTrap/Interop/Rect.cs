using System.Runtime.InteropServices;

namespace MouseTrap.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Rect
	{
		public int Left, Top, Right, Bottom;
	}
}
