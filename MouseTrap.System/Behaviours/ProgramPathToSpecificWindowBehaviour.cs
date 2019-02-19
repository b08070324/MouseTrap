using MouseTrap.Hooks;
using MouseTrap.Models;

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
				if (AppState.ProcessPath == e.ProcessPath)
				{
					AppState.WatchForSpecificWindow(e.Handle, false);
				}
			}
		}
	}
}
