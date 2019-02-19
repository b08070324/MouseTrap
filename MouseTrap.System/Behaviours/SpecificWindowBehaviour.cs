using MouseTrap.Hooks;
using MouseTrap.Interop;
using MouseTrap.Models;
using System;

namespace MouseTrap.Behaviours
{
	// If specific window is not picked, determine if specific window is picked
	// If picked, start all hooks
	internal class SpecificWindowBehaviour : BaseBehaviour
	{
		private IApplicationState AppState { get; }
		private IForegroundWindowHook ForegroundWindowHook { get; }
		private IWindowUpdateHook WindowUpdateHook { get; }
		private IMouseHook MouseHook { get; }

		public SpecificWindowBehaviour(IApplicationState appState,
			IForegroundWindowHook foregroundWindowHook,
			IWindowUpdateHook windowUpdateHook,
			IMouseHook mouseHook)
		{
			AppState = appState;
			ForegroundWindowHook = foregroundWindowHook;
			WindowUpdateHook = windowUpdateHook;
			MouseHook = mouseHook;
			AppState.WatchingSpecificWindow += AppState_WatchingSpecificProgram;
		}

		private void AppState_WatchingSpecificProgram(object sender, EventArgs e)
		{
			ForegroundWindowHook.StartHook();
			WindowUpdateHook.StartHook(AppState.Handle);

			// Get window rect
			NativeMethods.GetWindowRect(AppState.Handle, out Win32Rect rect);

			// Convert rect to dimensions
			Dimensions region = new Dimensions
			{
				Left = rect.Left,
				Top = rect.Top,
				Right = rect.Right,
				Bottom = rect.Bottom
			};

			// Get padded dimensions
			region = region.GetPaddedDimensions(AppState.Padding);

			// Start mouse hook
			MouseHook.StartHook(region);
		}
	}
}
