using MouseTrap.Hooks;
using MouseTrap.Interop;
using MouseTrap.Models;

using static System.Diagnostics.Debug;

namespace MouseTrap.Behaviours
{
	/// <summary>
	/// Determine if system variables change
	/// If changed, update TargetWindowDetails with changes
	/// </summary>
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

		/// <summary>
		/// When state changes to watching a specific window, get the window title and dimensions
		/// and update the target window details object for UI display
		/// </summary>
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

		/// <summary>
		/// When state changes to watching a program path, set target window details object
		/// with values that reflect the current state
		/// </summary>
		private void AppState_WatchingProgramPath(object sender, System.EventArgs e)
		{
			// Trace
			WriteLine($"{nameof(UpdateDetailsBehaviour)}.{nameof(AppState_WatchingProgramPath)}");

			// Set data
			TargetWindowDetails.WindowTitle = "Waiting for application";
			TargetWindowDetails.WindowDimensions = new Dimensions();
		}

		/// <summary>
		/// When the observed window title changes, update the target window details object
		/// </summary>
		private void WindowUpdateHook_TitleChanged(object sender, TitleChangedEventArgs e)
		{
			// Trace
			WriteLine($"{nameof(UpdateDetailsBehaviour)}.{nameof(WindowUpdateHook_TitleChanged)}");

			// Set title
			TargetWindowDetails.WindowTitle = e.Title;
		}

		/// <summary>
		/// When the observed window dimensions change, update the target window details object
		/// </summary>
		private void WindowUpdateHook_DimensionsChanged(object sender, DimensionsChangedEventArgs e)
		{
			// Trace
			WriteLine($"{nameof(UpdateDetailsBehaviour)}.{nameof(WindowUpdateHook_DimensionsChanged)}");

			// Set dimensions
			TargetWindowDetails.WindowDimensions = e.Dimensions;
		}

		/// <summary>
		/// When the foreground window changes, update the target window details object
		/// to show if the target window has focus
		/// </summary>
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
