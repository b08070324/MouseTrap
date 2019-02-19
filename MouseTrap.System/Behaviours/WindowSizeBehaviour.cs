using MouseTrap.Hooks;
using MouseTrap.Interop;
using MouseTrap.Models;

namespace MouseTrap.Behaviours
{
	// If specific window is picked, determine if window changes size
	// If changed, update MouseHook with size and padding
	internal class WindowSizeBehaviour : BaseBehaviour
	{
		private IApplicationState AppState { get; }
		private IWindowUpdateHook WindowUpdateHook { get; }
		private IMouseHook MouseHook { get; }

		public WindowSizeBehaviour(IApplicationState appState,
			IWindowUpdateHook windowUpdateHook,
			IMouseHook mouseHook)
		{
			AppState = appState;
			WindowUpdateHook = windowUpdateHook;
			MouseHook = mouseHook;
			WindowUpdateHook.DimensionsChanged += WindowUpdateHook_DimensionsChanged;
		}

		private void WindowUpdateHook_DimensionsChanged(object sender, DimensionsChangedEventArgs e)
		{
			if (AppState.IsWatchingSpecificWindow)
			{
				Dimensions region = e.Dimensions.GetPaddedDimensions(AppState.Padding);
				MouseHook.SetRegion(region);
			}
		}
	}
}
