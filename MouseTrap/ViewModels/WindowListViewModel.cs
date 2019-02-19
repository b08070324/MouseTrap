using MouseTrap.Foundation;
using MouseTrap.Models;
using System.Collections.ObjectModel;

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

		public virtual void RefreshList() { }

		public ObservableCollection<IWindowListItem> WindowList { get; private set; } = new ObservableCollection<IWindowListItem>();
	}
}
