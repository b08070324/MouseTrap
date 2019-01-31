using System;

namespace MouseTrap.Hooks
{
	public class ForegroundHookEventArgs : EventArgs
	{
		public IntPtr Handle { get; set; }
		public uint WindowThreadProcId { get; set; }
		public string ProcessPath { get; set; }
	}
}
