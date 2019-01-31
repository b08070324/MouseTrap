using System;
using System.Runtime.InteropServices;

namespace MouseTrap.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	public struct MSLLHOOKSTRUCT
	{
		public Point pt;
		public int mouseData;
		public int flags;
		public int time;
		public UIntPtr dwExtraInfo;
	}
}
