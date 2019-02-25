using MouseTrap.Hooks;
using MouseTrap.Models;

using static System.Diagnostics.Debug;

namespace MouseTrap.Behaviours
{
	/// <summary>
	/// If program path is picked, determine if a new foreground windows matches path
	/// If match, switch to watching specific window automatically
	/// </summary>
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
