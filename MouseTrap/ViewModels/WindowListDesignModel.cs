using MouseTrap.Models;

namespace MouseTrap.ViewModels
{
	public class WindowListDesignModel : WindowListViewModel
	{
		public WindowListDesignModel()
		{
			WindowList.Add(new WindowListItem { ProcessId = 123, ProcessPath = @"nope.exe", Title = "This is a test title", Width = 1920, Height = 1280 });
			WindowList.Add(new WindowListItem { ProcessId = 123, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 });
			WindowList.Add(new WindowListItem { ProcessId = 123, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 });

			SelectedWindow = WindowList[1];
		}
	}
}
