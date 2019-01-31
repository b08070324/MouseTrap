using System;
using System.Collections.ObjectModel;
using MouseTrap.Models;

namespace MouseTrap.WindowQuery
{
	public interface IWindowQueryManager
	{
		ObservableCollection<IWindowItem> GetWindowList();
		bool CheckWindow(IWindowItem windowItem);
		WindowItemUpdateDetails GetWindowItemUpdate(IWindowItem windowItem);
	}
}
