using MouseTrap.Hooks;
using MouseTrap.Models;
using System;

using static System.Diagnostics.Debug;

namespace MouseTrap.Behaviours
{
	/// <summary>
	/// If program path is not picked, determine if program path is picked 
	/// If picked, start ForegroundWindowHook
	/// </summary>
	internal class ProgramPathBehaviour : BaseBehaviour
	{
		private IApplicationState AppState { get; }
		private IForegroundWindowHook ForegroundWindowHook { get; }

		public ProgramPathBehaviour(IApplicationState appState, IForegroundWindowHook foregroundWindowHook)
		{
			AppState = appState;
			ForegroundWindowHook = foregroundWindowHook;
			AppState.WatchingProgramPath += AppState_WatchingProgramPath;
		}

		private void AppState_WatchingProgramPath(object sender, EventArgs e)
		{
			// Trace
			WriteLine($"{nameof(ProgramPathBehaviour)}.{nameof(AppState_WatchingProgramPath)}");

			// Start hook
			ForegroundWindowHook.StartHook();
		}
	}
}
