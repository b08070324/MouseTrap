using MouseTrap.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MouseTrap.ViewModels
{
	public abstract class WindowListViewModel : IViewModel, INotifyPropertyChanged
	{
		private IWindowListItem _selectedWindow;

		public IWindowListItem SelectedWindow
		{
			get => _selectedWindow;
			set
			{
				if (value != _selectedWindow)
				{
					_selectedWindow = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedWindow)));
				}
			}
		}

		public ObservableCollection<IWindowListItem> WindowList { get; private set; } = new ObservableCollection<IWindowListItem>();

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
