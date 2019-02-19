using MouseTrap.Models;
using System;
using System.ComponentModel;

using static System.Diagnostics.Debug;

namespace MouseTrap.ViewModels
{
	public class MainWindowLiveModel : MainWindowViewModel
	{
		public MainWindowLiveModel()
		{
			ToolBarViewModel = new ToolBarLiveModel();
			ToolBarViewModel.PropertyChanged += ToolBarViewModel_PropertyChanged;
			ToolBarViewModel.RefreshButtonClicked += ToolBarViewModel_RefreshButtonClicked;
			SetCurrentView(ViewType.WindowList);
		}

		// Changes the view by creating and setting the corresponding view model
		private void SetCurrentView(ViewType viewType)
		{
			switch (viewType)
			{
				case ViewType.WindowList:
					var windowListLiveModel = new WindowListLiveModel();
					windowListLiveModel.PropertyChanged += WindowListLiveModel_PropertyChanged;
					CurrentViewModel = windowListLiveModel;
					ToolBarViewModel.WindowLockEnabled = false;
					break;
				case ViewType.FindProgram:
					var findProgramLiveModel = new FindProgramLiveModel();
					findProgramLiveModel.PropertyChanged += FindProgramLiveModel_PropertyChanged;
					CurrentViewModel = findProgramLiveModel;
					ToolBarViewModel.WindowLockEnabled = false;
					break;
				case ViewType.LockWindow:
					CurrentViewModel = new LockWindowLiveModel();
					break;
				default:
					break;
			}
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
						WriteLine($"{windowListLiveModel.SelectedWindow.Title}");
						ToolBarViewModel.WindowLockEnabled = true;
					}
					else
					{
						WriteLine($"Window deselected");
						ToolBarViewModel.WindowLockEnabled = false;
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
						WriteLine($"{findProgramLiveModel.Filename}");
						ToolBarViewModel.WindowLockEnabled = true;
					}
					else
					{
						WriteLine($"Filename invalidated");
						ToolBarViewModel.WindowLockEnabled = false;
					}
				}
			}
		}
	}
}
