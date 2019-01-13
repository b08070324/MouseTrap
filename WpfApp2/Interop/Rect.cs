using System.Runtime.InteropServices;

namespace WpfApp2.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Rect
	{
		public int Left, Top, Right, Bottom;
	}
}
