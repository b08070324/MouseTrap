using System;

namespace MouseTrap.Models
{
	internal class EnumeratedWindow : IWindow
	{
		public IntPtr Handle { get; set; }
		public uint ProcessId { get; set; }
		public string ProcessPath { get; set; }
		public string Title { get; set; }
		public double Left { get; set; }
		public double Top { get; set; }
		public double Right { get; set; }
		public double Bottom { get; set; }
		public bool IsMinimized { get; set; }
	}
}
