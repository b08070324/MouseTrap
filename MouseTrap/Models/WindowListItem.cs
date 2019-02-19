using System;
using System.Windows.Media.Imaging;

namespace MouseTrap.Models
{
	public class WindowListItem : IWindowListItem
	{
		public IntPtr Handle { get; set; }
		public uint ProcessId { get; set; }
		public string ProcessPath { get; set; }
		public string Title { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public string ShortPath => System.IO.Path.GetFileName(ProcessPath);
		public BitmapSource ProcessIcon => WindowIcon.GetIcon(ProcessPath);
	}
}
