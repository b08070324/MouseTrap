using System;

namespace MouseTrap.Models
{
	public interface IWindow
	{
		IntPtr Handle { get; }
		uint ProcessId { get; }
		string ProcessPath { get; }
		string Title { get; }
		double Left { get; }
		double Top { get; }
		double Right { get; }
		double Bottom { get; }
		bool IsMinimized { get; }
	}
}
