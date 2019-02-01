using MouseTrap.Interop;
using System;

namespace MouseTrap.Hooks
{
	public class WindowRectEventArgs : EventArgs
	{
		public Win32Rect WindowRect { get; set; }
	}
}
