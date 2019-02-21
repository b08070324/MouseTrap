using MouseTrap.Models;
using System.Windows.Data;

namespace MouseTrap.ViewModels
{
	public class WindowListDesignModel : WindowListViewModel
	{
		public WindowListDesignModel()
		{
			WindowList.Add(new WindowListItem { ProcessId = 101, ProcessPath = @"nope.exe", Title = "This is a test title", Width = 1920, Height = 1280 });
			WindowList.Add(new WindowListItem { ProcessId = 102, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 });
			WindowList.Add(new WindowListItem { ProcessId = 103, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 });
			WindowList.Add(new WindowListItem { IsMinimized = true, ProcessId = 1001, ProcessPath = @"nope.exe", Title = "This is a test title", Width = 1920, Height = 1280 });
			WindowList.Add(new WindowListItem { IsMinimized = true, ProcessId = 1002, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 });
			WindowList.Add(new WindowListItem { IsMinimized = true, ProcessId = 1003, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 });

			SelectedWindow = WindowList[1];
		}
	}
}
