using System.Windows.Controls;
using System.Windows.Input;

namespace MouseTrap.Controls
{
	// This isn't ideal, but DependencyProperty makes setting 
	// a design-time context that works everywhere difficult
	public class BarButtonBase : UserControl
	{
		public BarButtonBase()
		{
			ButtonText = string.Empty;
			IsHighlighted = false;
		}

		public string ButtonText { get; set; }
		public string ButtonPadding { get; set; }
		public ICommand PressCommand { get; set; }
		public bool IsHighlighted { get; set; }
	}
}
