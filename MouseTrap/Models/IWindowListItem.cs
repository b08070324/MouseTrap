using System.Windows.Media.Imaging;

namespace MouseTrap.Models
{
	public interface IWindowListItem
	{
		BitmapSource ProcessIcon { get; }
		uint ProcessId { get; }
		string Title { get; }
		string ProcessPath { get; }
		string ShortPath { get; }
		double Width { get; }
		double Height { get; }
	}
}
