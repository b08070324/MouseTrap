using MouseTrap.Data;
using MouseTrap.Models;
using System;
using System.ComponentModel;

namespace MouseTrap.ViewModels
{
	public class MainWindowLiveModel : MainWindowViewModel
	{
		// Services
		private IApplicationSystem ApplicationSystem { get; set; }
		private Func<WindowListViewModel> WindowListViewModelFactory { get; set; }
		private Func<FindProgramViewModel> FindProgramViewModelFactory { get; set; }
		private Func<LockWindowViewModel> LockWindowViewModelFactory { get; set; }

		// Properties
		private string ProcessPath { get; set; }
		private uint ProcessId { get; set; }
		private IntPtr Handle { get; set; }
		private bool TargetingSpecificWindow { get; set; }

		public MainWindowLiveModel(
			IApplicationSystem applicationSystem, 
			Func<WindowListViewModel> windowListViewModelFactory,
			Func<FindProgramViewModel> findProgramViewModelFactory,
			Func<LockWindowViewModel> lockWindowViewModelFactory,
			Func<ToolBarViewModel> toolBarViewModelFactory)
		{
			// System
			ApplicationSystem = applicationSystem;
			ApplicationSystem.ApplicationState.WatchingCancelled += ApplicationState_WatchingCancelled;

			// Factories
			WindowListViewModelFactory = windowListViewModelFactory;
			FindProgramViewModelFactory = findProgramViewModelFactory;
			LockWindowViewModelFactory = lockWindowViewModelFactory;

			// Toolbar
			ToolBarViewModel = toolBarViewModelFactory();
			ToolBarViewModel.PropertyChanged += ToolBarViewModel_PropertyChanged;
			ToolBarViewModel.RefreshButtonClicked += ToolBarViewModel_RefreshButtonClicked;

			// Initialise
			SetWindowList();
		}

		private void StartWatch()
		{
			if (TargetingSpecificWindow && Handle != default) ApplicationSystem.ApplicationState.WatchForSpecificWindow(Handle);
			else if (!TargetingSpecificWindow && ProcessPath != default) ApplicationSystem.ApplicationState.WatchForProgramPath(ProcessPath);
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
			}
		}

		private void SetWindowList()
		{
			// Create view model
			var windowListLiveModel = WindowListViewModelFactory();
			if (ProcessId != default) windowListLiveModel.SelectedWindow = new WindowListItem { ProcessId = ProcessId };
			windowListLiveModel.RefreshList();
			windowListLiveModel.PropertyChanged += WindowListLiveModel_PropertyChanged;

			// Set view model
			CurrentViewModel = windowListLiveModel;

			// Update state
			TargetingSpecificWindow = true;
			ToolBarViewModel.WindowLockEnabled = (ProcessId != default);
		}

		private void SetFindProgram()
		{
			// Create view model
			var findProgramLiveModel = FindProgramViewModelFactory();
			if (ProcessPath != default) findProgramLiveModel.Filename = ProcessPath;
			findProgramLiveModel.PropertyChanged += FindProgramLiveModel_PropertyChanged;

			// Set view model
			CurrentViewModel = findProgramLiveModel;

			// Update state
			TargetingSpecificWindow = false;
			ToolBarViewModel.WindowLockEnabled = (ProcessPath != default);
		}

		private void SetLockWindow()
		{
			// Create view model
			var lockWindowLiveModel = LockWindowViewModelFactory();
			lockWindowLiveModel.ProcessPath = ProcessPath;

			// Set view model
			CurrentViewModel = lockWindowLiveModel;
		}

		// Stores copy of target window details to be sent to system
		private void UpdateTargetWindowDetails(string processPath = default, uint processId = default, IntPtr handle = default)
		{
			ProcessPath = processPath;
			ProcessId = processId;
			Handle = handle;
			ToolBarViewModel.WindowLockEnabled = (ProcessId != default || ProcessPath != default);
		}

		// Triggered when toolbar interaction changes its selected ViewType
		private void ToolBarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(ToolBarViewModel.CurrentView))
			{
				switch (ToolBarViewModel.CurrentView)
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
