using MouseTrap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseTrap.ViewModels
{
	public class WindowListDetailsDesignModel : WindowListItem
	{
		public WindowListDetailsDesignModel()
		{
			Handle = new IntPtr(0xA1);
			ProcessId = 101;
			ProcessPath = @"c:\windows\system32\notepad.exe";
			Title = "This is a test title";
			Width = 1920;
			Height = 1280;
		}
	}
}
