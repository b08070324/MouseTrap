using System.ComponentModel;

namespace MouseTrap.Models
{
	/// <summary>
	/// Observable object used to reflect changes in the targeted window
	/// Intended to enable window state changes to be presented to the user via the interface
	/// </summary>
	public interface ITargetWindowDetails : INotifyPropertyChanged
	{
		string WindowTitle { get; set; }
		Dimensions WindowDimensions { get; set; }
		bool HasFocus { get; set; }
	}
}
