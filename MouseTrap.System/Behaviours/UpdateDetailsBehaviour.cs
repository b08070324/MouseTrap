using MouseTrap.Hooks;
using MouseTrap.Interop;
using MouseTrap.Models;

namespace MouseTrap.Behaviours
{
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
			WindowUpdateHook.TitleChanged += WindowUpdateHook_TitleChanged;
			WindowUpdateHook.DimensionsChanged += WindowUpdateHook_DimensionsChanged;
			ForegroundWindowHook.ForegroundWindowChanged += ForegroundWindowHook_ForegroundWindowChanged;
		}

		private void AppState_WatchingSpecificWindow(object sender, System.EventArgs e)
		{
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

		private void WindowUpdateHook_TitleChanged(object sender, TitleChangedEventArgs e)
		{
			TargetWindowDetails.WindowTitle = e.Title;
		}

		private void WindowUpdateHook_DimensionsChanged(object sender, DimensionsChangedEventArgs e)
		{
			TargetWindowDetails.WindowDimensions = e.Dimensions;
		}

		private void ForegroundWindowHook_ForegroundWindowChanged(object sender, ForegroundWindowChangedEventArgs e)
		{
			TargetWindowDetails.HasFocus = (AppState.Handle == e.Handle &&
				AppState.ProcessId == e.WindowThreadProcId &&
				AppState.ProcessPath == e.ProcessPath);
		}
	}
}
