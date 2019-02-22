using MouseTrap.Interop;
using System;

namespace MouseTrap.Models
{
	internal class EnumeratedWindow : IWindow
	{
		private string _processPath;

		public IntPtr Handle { get; set; }
		public uint ProcessId { get; set; }
		public string Title { get; set; }
		public double Left { get; set; }
		public double Top { get; set; }
		public double Right { get; set; }
		public double Bottom { get; set; }
		public bool IsMinimized { get; set; }

		public string ProcessPath
		{
			get
			{
				if (_processPath == null) _processPath = NativeMethods.GetFullProcessName((int)ProcessId);
				return _processPath;
			}
		}
	}
}
