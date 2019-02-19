using MouseTrap.Data;
using MouseTrap.Models;
using System.Linq;

namespace MouseTrap.ViewModels
{
	public class WindowListLiveModel : WindowListViewModel
	{
		private IWindowCatalogue Catalogue { get; set; }

		public WindowListLiveModel(IWindowCatalogue catalogue)
		{
			Catalogue = catalogue;
		}

		public override void RefreshList()
		{
			// Store ref to selected window
			// SelectedWindow is cleared by view datagrid in some circumstances
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
				// Look for window with matching process ID
				var item = WindowList.FirstOrDefault(x => x.ProcessId == processId);

				// Set SelectedWindow to list entry, or clear
				SelectedWindow = item ?? null;
			}
		}
	}
}
