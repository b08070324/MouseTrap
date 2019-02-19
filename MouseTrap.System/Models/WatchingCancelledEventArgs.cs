using System;

namespace MouseTrap.Models
{
	public class WatchingCancelledEventArgs : EventArgs
	{
		public bool WindowWasClosed { get; set; }
	}
}
