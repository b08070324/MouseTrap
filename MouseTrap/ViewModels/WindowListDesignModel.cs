using MouseTrap.Models;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace MouseTrap.ViewModels
{
	public class WindowListDesignModel : WindowListViewModel
	{
		public WindowListDesignModel()
		{
			WindowList = new List<IWindowListItem>
			{
				new DesignWindowListItem(),
				new DesignWindowListItem(),
				new DesignWindowListItem()
			};

			SelectedWindow = WindowList[1];
		}

		private class DesignWindowListItem : IWindowListItem
		{
			public uint ProcessId => 123;
			public string ProcessPath => @"c:\windows\system32\notepad.exe";
			public string ShortPath => "notepad.exe";
			public BitmapSource ProcessIcon => WindowIcon.GetIcon(ProcessPath);
			public string Title => "This is a test title";
			public double Width => 1920;
			public double Height => 1280;
		}
	}
}
