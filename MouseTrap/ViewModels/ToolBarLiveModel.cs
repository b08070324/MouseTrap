using MouseTrap.Binding;
using MouseTrap.Models;

namespace MouseTrap.ViewModels
{
	public class ToolBarLiveModel : ToolBarViewModel
	{
		private IApplicationState ApplicationState { get; set; }
		private ViewType PreviousView { get; set; }

		public ToolBarLiveModel(IApplicationState applicationState)
		{
			// App state
			ApplicationState = applicationState;
			ApplicationState.WatchingCancelled += ApplicationState_WatchingCancelled;

			// Commands
			ChooseWindowCommand = new RelayCommand(p => CurrentView = ViewType.WindowList, p => (CurrentView == ViewType.FindProgram));
			FindProgramCommand = new RelayCommand(p => CurrentView = ViewType.FindProgram, p => (CurrentView == ViewType.WindowList));
			ToggleLockCommand = new RelayCommand(p => ToggleLockWindow(), p => WindowLockEnabled);
			RefreshListCommand = new RelayCommand(p => OnRefreshButtonClicked());
		}

		~ToolBarLiveModel()
		{
			ApplicationState.WatchingCancelled -= ApplicationState_WatchingCancelled;
		}

		// Toggles the lock window back to previous view if the target window was closed
		private void ApplicationState_WatchingCancelled(object sender, WatchingCancelledEventArgs e)
		{
			if (e.WindowWasClosed)
			{
				ToggleLockWindow();
			}
		}

		// Toggles between the lock window and either of the program selection views
		private void ToggleLockWindow()
		{
			if (CurrentView == ViewType.LockWindow)
			{
				// Return to previous view
				CurrentView = PreviousView;
			}
			else
			{
				// Store current view model
				if (CurrentView == ViewType.WindowList) PreviousView = ViewType.WindowList;
				else PreviousView = ViewType.FindProgram;

				// Switch to lock window view
				CurrentView = ViewType.LockWindow;
			}
		}
	}
}
