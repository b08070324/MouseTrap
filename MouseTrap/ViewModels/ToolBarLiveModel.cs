using MouseTrap.Binding;
using MouseTrap.Models;

namespace MouseTrap.ViewModels
{
	public class ToolBarLiveModel : ToolBarViewModel
	{
		private ViewType PreviousView { get; set; }

		public ToolBarLiveModel()
		{
			ChooseWindowCommand = new RelayCommand(p => CurrentView = ViewType.WindowList, p => (CurrentView == ViewType.FindProgram));
			FindProgramCommand = new RelayCommand(p => CurrentView = ViewType.FindProgram, p => (CurrentView == ViewType.WindowList));
			ToggleLockCommand = new RelayCommand(p => ToggleLockWindow(), p => WindowLockEnabled);
			RefreshListCommand = new RelayCommand(p => OnRefreshButtonClicked());
		}

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
