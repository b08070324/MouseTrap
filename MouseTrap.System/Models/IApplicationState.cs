using System;

namespace MouseTrap.Models
{
	public interface IApplicationState
	{
		// Properties
		IntPtr Handle { get; }
		uint ProcessId { get; }
		string ProcessPath { get; }
		Dimensions Padding { get; }
		bool IsWatchingProgramPath { get; }
		bool IsWatchingSpecificWindow { get; }

		// Commands
		void WatchForProgramPath(string processPath);
		void WatchForSpecificWindow(IntPtr handle, bool startRestricted);
		void CancelWatch();
		void SetPadding(Dimensions padding);

		// Events
		event EventHandler WatchingProgramPath;
		event EventHandler WatchingSpecificWindow;
		event EventHandler WatchingCancelled;
		event EventHandler PaddingUpdated;
	}
}
