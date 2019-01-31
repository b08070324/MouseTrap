using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MouseTrap.Models
{
	public interface IWindowItem : IEquatable<IWindowItem>
	{
		IntPtr Handle { get; set; }
		uint ProcessId { get; set; }
		string ProcessPath { get; set; }
		string Title { get; set; }
		Dimensions Dimensions { get; set; }

		double Width { get; }
		double Height { get; }
		string ShortPath { get; }
		BitmapSource ProcessIcon { get; }

		bool IsValid { get; }
		bool IsPathValid { get; }
		bool IsMatch(IWindowItem item);
		bool MatchByProcessPath(IWindowItem item);
		IWindowItem Clone();
	}
}
