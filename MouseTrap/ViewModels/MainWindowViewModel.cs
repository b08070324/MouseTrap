using MouseTrap.Foundation;
using System.ComponentModel;

namespace MouseTrap.ViewModels
{
	public abstract class MainWindowViewModel : NotifyingObject
	{
		private IViewModel _currentViewModel;

		public IViewModel CurrentViewModel
		{
			get => _currentViewModel;
			set => SetAndRaiseEvent(ref _currentViewModel, value);
		}

		public ToolBarViewModel ToolBarViewModel { get; set; }
	}
}
