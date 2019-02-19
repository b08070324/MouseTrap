using MouseTrap.Hooks;
using MouseTrap.Models;

using static System.Diagnostics.Debug;

namespace MouseTrap.Behaviours
{
	// If program path is picked, determine if fg change matches path
	// If match, pick specific window automatically
	internal class ProgramPathToSpecificWindowBehaviour : BaseBehaviour
	{
		private IApplicationState AppState { get; }
		private IForegroundWindowHook ForegroundWindowHook { get; }

		public ProgramPathToSpecificWindowBehaviour(IApplicationState appState, IForegroundWindowHook foregroundWindowHook)
		{
			AppState = appState;
			ForegroundWindowHook = foregroundWindowHook;
			ForegroundWindowHook.ForegroundWindowChanged += ForegroundWindowHook_ForegroundWindowChanged;
		}

		private void ForegroundWindowHook_ForegroundWindowChanged(object sender, ForegroundWindowChangedEventArgs e)
		{
			if (AppState.IsWatchingProgramPath)
			{
				if (string.Compare(AppState.ProcessPath, e.ProcessPath, true) == 0)
				{
					// Trace
					WriteLine($"{nameof(ProgramPathToSpecificWindowBehaviour)}.{nameof(ForegroundWindowHook_ForegroundWindowChanged)}");

					// Set state
					AppState.WatchForSpecificWindow(e.Handle);
				}
			}
		}
	}
}
