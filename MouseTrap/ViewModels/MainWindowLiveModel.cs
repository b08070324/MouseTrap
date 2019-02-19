using MouseTrap.Models;
using System;
using System.ComponentModel;

namespace MouseTrap.ViewModels
{
	public class MainWindowLiveModel : MainWindowViewModel
	{
		private IApplicationSystem ApplicationSystem { get; set; }
		private string ProcessPath { get; set; }
		private uint ProcessId { get; set; }
		private IntPtr Handle { get; set; }
		private bool TargetingSpecificWindow => (ProcessPath != null && ProcessId != 0 && Handle != default);
		private bool TargetingProgramPath => (ProcessPath != null && ProcessId == 0 && Handle == default);

		public MainWindowLiveModel()
		{
			ApplicationSystem = ApplicationSystemFactory.GetApplicationSystem();
			ApplicationSystem.ApplicationState.WatchingCancelled += ApplicationState_WatchingCancelled;

			ToolBarViewModel = new ToolBarLiveModel();
			ToolBarViewModel.PropertyChanged += ToolBarViewModel_PropertyChanged;
			ToolBarViewModel.RefreshButtonClicked += ToolBarViewModel_RefreshButtonClicked;

			SetCurrentView(ViewType.WindowList);
		}

		private void StartWatch()
		{
			if (TargetingSpecificWindow) ApplicationSystem.ApplicationState.WatchForSpecificWindow(Handle);
			else if (TargetingProgramPath) ApplicationSystem.ApplicationState.WatchForProgramPath(ProcessPath);
		}

		private void StopWatch()
		{
			ApplicationSystem.ApplicationState.CancelWatch();
		}

		private void ApplicationState_WatchingCancelled(object sender, WatchingCancelledEventArgs e)
		{
			// If the window was closed clear target details
			if (e.WindowWasClosed)
			{
				UpdateTargetWindowDetails();
				if (ToolBarViewModel is ToolBarLiveModel toolBarLiveModel) toolBarLiveModel.ToggleLockWindow();
			}
		}

		// Changes the view by creating and setting the corresponding view model
		private void SetCurrentView(ViewType viewType)
		{
			switch (viewType)
			{
				case ViewType.WindowList:
					StopWatch();
					SetWindowList();
					break;
				case ViewType.FindProgram:
					StopWatch();
					SetFindProgram();
					break;
				case ViewType.LockWindow:
					StartWatch();
					SetLockWindow();
					break;
				default:
					break;
			}
		}

		private void SetWindowList()
		{
			// Create view model
			var windowListLiveModel = new WindowListLiveModel();
			if (TargetingSpecificWindow) windowListLiveModel.SelectedWindow = new WindowListItem { ProcessId = ProcessId };
			windowListLiveModel.RefreshList();
			windowListLiveModel.PropertyChanged += WindowListLiveModel_PropertyChanged;

			// Set view model
			CurrentViewModel = windowListLiveModel;

			// Update toolbar
			ToolBarViewModel.WindowLockEnabled = TargetingSpecificWindow;
		}

		private void SetFindProgram()
		{
			// Create view model
			var findProgramLiveModel = new FindProgramLiveModel();
			if (TargetingProgramPath) findProgramLiveModel.Filename = ProcessPath;
			findProgramLiveModel.PropertyChanged += FindProgramLiveModel_PropertyChanged;

			// Set view model
			CurrentViewModel = findProgramLiveModel;

			// Update toolbar
			ToolBarViewModel.WindowLockEnabled = TargetingProgramPath;
		}

		private void SetLockWindow()
		{
			// Create view model
			var lockWindowLiveModel = new LockWindowLiveModel(ApplicationSystem.TargetWindowDetails);
			lockWindowLiveModel.ProcessPath = ProcessPath;

			// Set view model
			CurrentViewModel = lockWindowLiveModel;
		}

		// Stores copy of target window details to be sent to system
		private void UpdateTargetWindowDetails(string processPath = null, uint processId = 0, IntPtr handle = default)
		{
			ProcessPath = processPath;
			ProcessId = processId;
			Handle = handle;
			ToolBarViewModel.WindowLockEnabled = (TargetingSpecificWindow || TargetingProgramPath);
		}

		// Triggered when toolbar interaction changes its selected ViewType
		private void ToolBarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(ToolBarViewModel.CurrentView))
			{
				SetCurrentView(ToolBarViewModel.CurrentView);
			}
		}

		// Triggered when the toolbar refresh button is activated
		private void ToolBarViewModel_RefreshButtonClicked(object sender, EventArgs e)
		{
			if (CurrentViewModel is WindowListLiveModel windowListViewModel)
			{
				windowListViewModel.RefreshList();
			}
		}

		// Triggered when the selected item in WindowListView changes
		private void WindowListLiveModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(WindowListLiveModel.SelectedWindow))
			{
				if (CurrentViewModel is WindowListLiveModel windowListLiveModel)
				{
					if (windowListLiveModel.SelectedWindow != null)
					{
						UpdateTargetWindowDetails(windowListLiveModel.SelectedWindow.ProcessPath,
							windowListLiveModel.SelectedWindow.ProcessId,
							windowListLiveModel.SelectedWindow.Handle);
					}
					else
					{
						UpdateTargetWindowDetails();
					}
				}
			}
		}

		// Triggered when the filename in FindProgramView changes
		private void FindProgramLiveModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(FindProgramLiveModel.IsFilenameValid))
			{
				if (CurrentViewModel is FindProgramLiveModel findProgramLiveModel)
				{
					if (findProgramLiveModel.IsFilenameValid)
					{
						UpdateTargetWindowDetails(findProgramLiveModel.Filename);
					}
					else
					{
						UpdateTargetWindowDetails();
					}
				}
			}
		}
	}
}
