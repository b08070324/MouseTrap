using System;
using System.Collections.ObjectModel;
using System.Windows;
using MouseTrap.Models;

namespace MouseTrap.WindowQuery
{
	public class DesignWindowQueryManager : IWindowQueryManager
	{
		private static string tmpFilename = @"D:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe";

		public ObservableCollection<IWindowItem> GetWindowList()
		{
			var list = new ObservableCollection<IWindowItem>
			{
				new WindowItem
				{
					Handle = new IntPtr(1),
					ProcessId = 1001,
					ProcessPath = tmpFilename,
					Title = "Window 1 - Lorem ipsum dolor sit amet",
					Dimensions = new Dimensions(0,0,1920,1080)
				},

				new WindowItem { ProcessPath = tmpFilename, Title = "Window 2 - Morbi vulputate laoreet pulvinar", ProcessId = 1002, Dimensions = new Dimensions(0,0,1920,1080) },
				new WindowItem { ProcessPath = tmpFilename, Title = "Window 3 - Mauris at justo sagittis, efficitur felis id", ProcessId = 1003, Dimensions = new Dimensions(0,0,1920,1080) },
				new WindowItem { ProcessPath = tmpFilename, Title = "Window 4 - Vivamus convallis lorem in libero vestibulum", ProcessId = 1004, Dimensions = new Dimensions(0,0,1920,1080) },
				new WindowItem { ProcessPath = tmpFilename, Title = "Window 5 - Orci varius natoque penatibus et magnis", ProcessId = 1005, Dimensions = new Dimensions(0,0,1920,1080) },
				new WindowItem { ProcessPath = tmpFilename, Title = "Window 6 - Suspendisse tincidunt tristique mauris, eget rhoncus arcu vestibulum at", ProcessId = 1006, Dimensions = new Dimensions(0,0,1920,1080) },
				new WindowItem { ProcessPath = tmpFilename, Title = "Window 7 - Donec fermentum maximus ultrices. Nunc ultricies, dui eu faucibus imperdiet, massa diam elementum mi, et tristique quam ipsum eget sem", ProcessId = 1007, Dimensions = new Dimensions(0,0,1920,1080) },
				new WindowItem { ProcessPath = tmpFilename, Title = "Window 8", ProcessId = 1008, Dimensions = new Dimensions(0,0,1920,1080) },
				new WindowItem { ProcessPath = tmpFilename, Title = "Window 9 - Lorem ipsum", ProcessId = 1009, Dimensions = new Dimensions(0,0,1920,1080) },
				new WindowItem { ProcessPath = tmpFilename, Title = "Window 10 - Morbi vulputate laoreet", ProcessId = 1010, Dimensions = new Dimensions(0,0,1920,1080) },
				new WindowItem { ProcessPath = tmpFilename, Title = "Window 11 - Orci varius natoque", ProcessId = 1011, Dimensions = new Dimensions(0,0,1920,1080) },
				new WindowItem { ProcessPath = tmpFilename, Title = "Window 12 - Donec", ProcessId = 1012, Dimensions = new Dimensions(0,0,1920,1080) }
			};

			return list;
		}

		public bool CheckWindow(IWindowItem windowItem)
		{
			return true;
		}

		public WindowItemUpdateDetails GetWindowItemUpdate(IWindowItem windowItem)
		{
			return new WindowItemUpdateDetails
			{
				Title = "Window 1 - Lorem ipsum dolor sit amet",
				Dimensions = new Dimensions(0,0,1920,1080)
			};
		}

	}
}
