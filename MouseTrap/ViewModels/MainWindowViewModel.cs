using System.ComponentModel;

namespace MouseTrap.ViewModels
{
	public abstract class MainWindowViewModel : INotifyPropertyChanged
	{
		private IViewModel _currentViewModel;

		public IViewModel CurrentViewModel
		{
			get => _currentViewModel;
			set
			{
				_currentViewModel = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentViewModel)));
			} 
		}

		public ToolBarViewModel ToolBarViewModel { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
