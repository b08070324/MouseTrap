using MouseTrap.Foundation;
using MouseTrap.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MouseTrap.ViewModels
{
	public abstract class WindowListViewModel : NotifyingObject, IViewModel
	{
		private IWindowListItem _selectedWindow;

		public IWindowListItem SelectedWindow
		{
			get => _selectedWindow;
			set => SetAndRaiseEvent(ref _selectedWindow, value);
		}

		public ObservableCollection<IWindowListItem> WindowList { get; private set; } = new ObservableCollection<IWindowListItem>();
	}
}
