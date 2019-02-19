using System.ComponentModel;

namespace MouseTrap.Models
{
	public interface ITargetWindowDetails : INotifyPropertyChanged
	{
		string WindowTitle { get; set; }
		Dimensions WindowDimensions { get; set; }
		bool HasFocus { get; set; }
	}
}
