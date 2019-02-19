using MouseTrap.Models;
using System.Collections.Generic;

namespace MouseTrap.ViewModels
{
	public abstract class WindowListViewModel : IViewModel
	{
		public IList<IWindowListItem> WindowList { get; protected set; }
		public IWindowListItem SelectedWindow { get; set; }
	}
}
