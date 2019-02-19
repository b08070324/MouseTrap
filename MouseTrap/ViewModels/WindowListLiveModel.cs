using MouseTrap.Data;
using MouseTrap.Models;
using System.Linq;

namespace MouseTrap.ViewModels
{
	public class WindowListLiveModel : WindowListViewModel
	{
		private WindowCatalogue Catalogue { get; set; }

		public WindowListLiveModel()
		{
			Catalogue = new WindowCatalogue();
		}

		public void RefreshList()
		{
			// Store ref to selected window
			uint processId = SelectedWindow == null ? 0 : SelectedWindow.ProcessId;

			// Refresh list
			WindowList.Clear();
			Catalogue.EnumerateWindows(window =>
			{
				WindowList.Add(new WindowListItem
				{
					Handle = window.Handle,
					ProcessId = window.ProcessId,
					ProcessPath = window.ProcessPath,
					Title = window.Title,
					Width = window.Right - window.Left,
					Height = window.Bottom - window.Top,
				});
			});

			// Reset selected window
			if (processId != 0)
			{
				var item = WindowList.FirstOrDefault(x => x.ProcessId == processId);
				if (item != null) SelectedWindow = item;
			}
		}
	}
}
