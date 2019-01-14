using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Interop
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
