using MouseTrap.Data;
using MouseTrap.Models;
using System;
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
			uint processId = default;
			IntPtr handle = default;

			if (SelectedWindow != null)
			{
				processId = SelectedWindow.ProcessId;
				handle = SelectedWindow.Handle;
			}

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
					IsMinimized = window.IsMinimized
				});
			});

			// Reset selected window
			if (processId != default && handle != default)
			{
				// Look for window with matching process ID
				var item = WindowList.FirstOrDefault(x => x.ProcessId == processId && x.Handle == handle);

				// Set SelectedWindow to list entry, or clear
				SelectedWindow = item ?? null;
			}
		}
	}
}
