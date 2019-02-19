using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace MouseTrap.Models
{
	public static class WindowIcon
	{
		private static readonly BitmapImage DefaultIcon;

		static WindowIcon()
		{
			DefaultIcon = new BitmapImage(new Uri("/MouseTrap;component/Resources/DefaultListIcon.png", UriKind.Relative));
		}

		public static BitmapSource GetIcon(string processPath)
		{
			BitmapSource result = DefaultIcon;

			if (processPath != null && processPath.Length > 0 && System.IO.File.Exists(processPath))
			{
				using (var ico = System.Drawing.Icon.ExtractAssociatedIcon(processPath))
				{
					result = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
				}
			}

			return result;
		}
	}
}
