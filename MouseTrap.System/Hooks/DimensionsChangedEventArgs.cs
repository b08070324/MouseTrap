using MouseTrap.Models;
using System;

namespace MouseTrap.Hooks
{
	internal class DimensionsChangedEventArgs : EventArgs
	{
		public Dimensions Dimensions { get; set; }
	}
}
