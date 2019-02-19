using MouseTrap.Models;
using System;
using System.ComponentModel;

namespace MouseTrap.ViewModels
{
	public class MainWindowLiveModel : MainWindowViewModel
	{
		public MainWindowLiveModel()
		{
			CurrentViewModel = new WindowListLiveModel();
			ToolBarViewModel = new ToolBarLiveModel();
			ToolBarViewModel.PropertyChanged += ToolBarViewModel_PropertyChanged;
			ToolBarViewModel.RefreshButtonClicked += ToolBarViewModel_RefreshButtonClicked;
		}

		private void ToolBarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(ToolBarViewModel.CurrentView))
			{
				switch (ToolBarViewModel.CurrentView)
				{
					case ViewType.WindowList:
						CurrentViewModel = new WindowListLiveModel();
						break;
					case ViewType.FindProgram:
						CurrentViewModel = new FindProgramLiveModel();
						break;
					case ViewType.LockWindow:
						CurrentViewModel = new LockWindowLiveModel();
						break;
					default:
						break;
				}
			}
		}

		private void ToolBarViewModel_RefreshButtonClicked(object sender, EventArgs e)
		{
			if (CurrentViewModel is WindowListLiveModel windowListViewModel)
			{
				windowListViewModel.RefreshList();
			}
		}
	}
}
