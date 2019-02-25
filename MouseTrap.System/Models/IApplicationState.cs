using System;

namespace MouseTrap.Models
{
	/// <summary>
	/// Application state holds the current goal of the system
	/// Commands can be executed by both the UI, and system behaviours acting on hook events
	/// Changes in state are observed by UI and system behaviours
	/// </summary>
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
		void WatchForSpecificWindow(IntPtr handle);
		void CancelWatch(bool windowWasClosed = false);
		void SetPadding(Dimensions padding);
		void SetPadding(double left, double top, double right, double bottom);

		// Events
		event EventHandler WatchingProgramPath;
		event EventHandler WatchingSpecificWindow;
		event EventHandler<WatchingCancelledEventArgs> WatchingCancelled;
		event EventHandler PaddingUpdated;
	}
}
