using MouseTrap.Hooks;
using MouseTrap.Interop;
using MouseTrap.Models;

using static System.Diagnostics.Debug;

namespace MouseTrap.Behaviours
{
	// Determine if system variables change
	// If changed, update TargetWindowDetails with changes
	internal class UpdateDetailsBehaviour : BaseBehaviour
	{
		private IApplicationState AppState { get; }
		private ITargetWindowDetails TargetWindowDetails { get; }
		private IWindowUpdateHook WindowUpdateHook { get; }
		private IForegroundWindowHook ForegroundWindowHook { get; }

		public UpdateDetailsBehaviour(IApplicationState appState,
			ITargetWindowDetails targetWindowDetails,
			IWindowUpdateHook windowUpdateHook,
			IForegroundWindowHook foregroundWindowHook)
		{
			AppState = appState;
			TargetWindowDetails = targetWindowDetails;
			WindowUpdateHook = windowUpdateHook;
			ForegroundWindowHook = foregroundWindowHook;

			AppState.WatchingSpecificWindow += AppState_WatchingSpecificWindow;
			AppState.WatchingProgramPath += AppState_WatchingProgramPath;

			WindowUpdateHook.TitleChanged += WindowUpdateHook_TitleChanged;
			WindowUpdateHook.DimensionsChanged += WindowUpdateHook_DimensionsChanged;

			ForegroundWindowHook.ForegroundWindowChanged += ForegroundWindowHook_ForegroundWindowChanged;
		}

		private void AppState_WatchingSpecificWindow(object sender, System.EventArgs e)
		{
			// Trace
			WriteLine($"{nameof(UpdateDetailsBehaviour)}.{nameof(AppState_WatchingSpecificWindow)}");

			// Set data
			TargetWindowDetails.WindowTitle = NativeMethods.GetWindowText(AppState.Handle);

			NativeMethods.GetWindowRect(AppState.Handle, out Win32Rect rect);
			TargetWindowDetails.WindowDimensions = new Dimensions
			{
				Left = rect.Left,
				Top = rect.Top,
				Right = rect.Right,
				Bottom = rect.Bottom
			};
		}

		private void AppState_WatchingProgramPath(object sender, System.EventArgs e)
		{
			// Trace
			WriteLine($"{nameof(UpdateDetailsBehaviour)}.{nameof(AppState_WatchingProgramPath)}");

			// Set data
			TargetWindowDetails.WindowTitle = "Waiting for application";
			TargetWindowDetails.WindowDimensions = new Dimensions();
		}

		private void WindowUpdateHook_TitleChanged(object sender, TitleChangedEventArgs e)
		{
			// Trace
			WriteLine($"{nameof(UpdateDetailsBehaviour)}.{nameof(WindowUpdateHook_TitleChanged)}");

			// Set title
			TargetWindowDetails.WindowTitle = e.Title;
		}

		private void WindowUpdateHook_DimensionsChanged(object sender, DimensionsChangedEventArgs e)
		{
			// Trace
			WriteLine($"{nameof(UpdateDetailsBehaviour)}.{nameof(WindowUpdateHook_DimensionsChanged)}");

			// Set dimensions
			TargetWindowDetails.WindowDimensions = e.Dimensions;
		}

		private void ForegroundWindowHook_ForegroundWindowChanged(object sender, ForegroundWindowChangedEventArgs e)
		{
			// Trace
			WriteLine($"{nameof(UpdateDetailsBehaviour)}.{nameof(ForegroundWindowHook_ForegroundWindowChanged)}");

			// Set focus state
			TargetWindowDetails.HasFocus = (AppState.Handle == e.Handle &&
				AppState.ProcessId == e.WindowThreadProcId &&
				AppState.ProcessPath == e.ProcessPath);
		}
	}
}
