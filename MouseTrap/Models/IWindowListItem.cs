using System;
using System.Windows.Media.Imaging;

namespace MouseTrap.Models
{
	public interface IWindowListItem
	{
		IntPtr Handle { get; }
		uint ProcessId { get; }
		string ProcessPath { get; }
		string Title { get; }
		double Width { get; }
		double Height { get; }
		string ShortPath { get; }
		BitmapSource ProcessIcon { get; }
	}
}
