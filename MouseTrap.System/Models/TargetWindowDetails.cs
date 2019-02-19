using System.ComponentModel;

namespace MouseTrap.Models
{
	internal class TargetWindowDetails : ITargetWindowDetails
	{
		private string _windowTitle;
		private Dimensions _windowDimensions;
		private bool _hasFocus;

		public string WindowTitle
		{
			get => _windowTitle;
			set
			{
				_windowTitle = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowTitle)));
			}
		}

		public Dimensions WindowDimensions
		{
			get => _windowDimensions;
			set
			{
				_windowDimensions = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowDimensions)));
			}
		}

		public bool HasFocus
		{
			get => _hasFocus;
			set
			{
				_hasFocus = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasFocus)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
