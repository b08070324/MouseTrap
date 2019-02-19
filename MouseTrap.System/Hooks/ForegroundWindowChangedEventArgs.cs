﻿using System;

namespace MouseTrap.Hooks
{
	internal class ForegroundWindowChangedEventArgs : EventArgs
	{
		public IntPtr Handle { get; set; }
		public uint WindowThreadProcId { get; set; }
		public string ProcessPath { get; set; }
	}
}
