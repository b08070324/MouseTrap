using System.Runtime.InteropServices;

namespace WpfApp2.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Point
	{
		public int X;
		public int Y;

		public override string ToString()
		{
			return string.Format("{0}, {1}", X, Y);
		}
	}
}
