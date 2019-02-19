using System.Runtime.InteropServices;

namespace MouseTrap.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct Win32Rect
	{
		public int Left, Top, Right, Bottom;
	}
}
