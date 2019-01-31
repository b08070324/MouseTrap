using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace MouseTrap.Models
{
	public static class WindowIcon
	{
		private static readonly BitmapImage defaultIcon = new BitmapImage(
			new Uri("/MouseTrap;component/Resources/AppIcon.ico", UriKind.Relative));

		public static BitmapSource GetIcon(string processPath)
		{
			BitmapSource result = defaultIcon;

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
