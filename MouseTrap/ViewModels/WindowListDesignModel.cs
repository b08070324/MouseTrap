using MouseTrap.Models;
using System.Windows.Data;

namespace MouseTrap.ViewModels
{
	public class WindowListDesignModel : WindowListViewModel
	{
		public WindowListDesignModel()
		{
			WindowList.Add(new WindowListItem { Handle = new System.IntPtr(0xA1), ProcessId = 101, ProcessPath = @"nope.exe", Title = "This is a test title", Width = 1920, Height = 1280 });
			WindowList.Add(new WindowListItem { Handle = new System.IntPtr(0xA2), ProcessId = 102, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 });
			WindowList.Add(new WindowListItem { Handle = new System.IntPtr(0xA3), ProcessId = 103, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 });
			WindowList.Add(new WindowListItem { IsMinimized = true, Handle = new System.IntPtr(0xb1), ProcessId = 1001, ProcessPath = @"nope.exe", Title = "This is a test title", Width = 1920, Height = 1280 });
			WindowList.Add(new WindowListItem { IsMinimized = true, Handle = new System.IntPtr(0xb2), ProcessId = 1002, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 });
			WindowList.Add(new WindowListItem { IsMinimized = true, Handle = new System.IntPtr(0xb3), ProcessId = 1003, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 });

			SelectedWindow = WindowList[1];
		}

		public override void RefreshList()
		{
			throw new System.NotImplementedException();
		}
	}
}
