using System;
using System.Runtime.InteropServices;

namespace MouseTrap.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	public struct MSLLHOOKSTRUCT
	{
		public Point pt;
		public int mouseData; // be careful, this must be ints, not uints (was wrong before I changed it...). regards, cmew.
		public int flags;
		public int time;
		public UIntPtr dwExtraInfo;
	}
}
