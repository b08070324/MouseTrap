using MouseTrap.Hooks;
using MouseTrap.Models;
using System;

using static System.Diagnostics.Debug;

namespace MouseTrap.Behaviours
{
	// If specific window is picked, determine if window is closed
	// If closed, cancel watch
	internal class WindowClosedBehaviour : BaseBehaviour
	{
		private IApplicationState AppState { get; }
		private IWindowUpdateHook WindowUpdateHook { get; }

		public WindowClosedBehaviour(IApplicationState appState, IWindowUpdateHook windowUpdateHook)
		{
			AppState = appState;
			WindowUpdateHook = windowUpdateHook;
			WindowUpdateHook.WindowClosed += WindowUpdateHook_WindowClosed;
		}

		private void WindowUpdateHook_WindowClosed(object sender, EventArgs e)
		{
			// Trace
			WriteLine($"{nameof(WindowClosedBehaviour)}.{nameof(WindowUpdateHook_WindowClosed)}");

			// Set state
			AppState.CancelWatch(true);
		}
	}
}
