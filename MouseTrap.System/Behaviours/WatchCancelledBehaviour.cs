using MouseTrap.Hooks;
using MouseTrap.Models;
using System;

namespace MouseTrap.Behaviours
{
	// If specific window is picked, determine if pick is cancelled
	// If cancelled, stop all hooks
	internal class WatchCancelledBehaviour : BaseBehaviour
	{
		private IApplicationState AppState { get; }
		private IForegroundWindowHook ForegroundWindowHook { get; }
		private IWindowUpdateHook WindowUpdateHook { get; }
		private IMouseHook MouseHook { get; }

		public WatchCancelledBehaviour(IApplicationState appState,
			IForegroundWindowHook foregroundWindowHook,
			IWindowUpdateHook windowUpdateHook,
			IMouseHook mouseHook)
		{
			AppState = appState;
			ForegroundWindowHook = foregroundWindowHook;
			WindowUpdateHook = windowUpdateHook;
			MouseHook = mouseHook;
			AppState.WatchingCancelled += AppState_WatchingCancelled;
		}

		private void AppState_WatchingCancelled(object sender, EventArgs e)
		{
			ForegroundWindowHook.StopHook();
			WindowUpdateHook.StopHook();
			MouseHook.StopHook();
		}
	}
}
