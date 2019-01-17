using System;

namespace MouseTrap.Foundation
{
	// Reduced data for listing many windows
	public class WindowItem
	{
		public IntPtr Handle { get; set; }
		public uint ProcessId { get; set; }
		public string ProcessName { get; set; }
		public string Title { get; set; }
	}
}
