using MouseTrap.Hooks;
using MouseTrap.Models;

namespace MouseTrap.Behaviours
{
	// If specific window is picked, determine if specific window is in focus
	// If focus changes, update MouseHook
	internal class WindowFocusBehaviour : BaseBehaviour
	{
		private IApplicationState AppState { get; }
		private IForegroundWindowHook ForegroundWindowHook { get; }
		private IMouseHook MouseHook { get; }

		public WindowFocusBehaviour(IApplicationState appState,
			IForegroundWindowHook foregroundWindowHook,
			IMouseHook mouseHook)
		{
			AppState = appState;
			ForegroundWindowHook = foregroundWindowHook;
			MouseHook = mouseHook;
			ForegroundWindowHook.ForegroundWindowChanged += ForegroundWindowHook_ForegroundWindowChanged;
		}

		private void ForegroundWindowHook_ForegroundWindowChanged(object sender, ForegroundWindowChangedEventArgs e)
		{
			if (AppState.IsWatchingSpecificWindow)
			{
				MouseHook.SetState(AppState.Handle == e.Handle &&
					AppState.ProcessId == e.WindowThreadProcId &&
					AppState.ProcessPath == e.ProcessPath);
			}
		}
	}
}
