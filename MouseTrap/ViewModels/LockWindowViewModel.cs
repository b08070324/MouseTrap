using MouseTrap.Foundation;
using System.ComponentModel;

namespace MouseTrap.ViewModels
{
	public abstract class LockWindowViewModel : NotifyingObject, IViewModel
	{
		private string _title = string.Empty;
		private bool _windowIsFocused;
		private double _windowHeight;
		private double _windowWidth;

		public string Title
		{
			get => _title;
			set => SetAndRaiseEvent(ref _title, value);
		}

		public bool WindowIsFocused
		{
			get => _windowIsFocused;
			set => SetAndRaiseEvent(ref _windowIsFocused, value);
		}

		public double WindowHeight
		{
			get => _windowHeight;
			set => SetAndRaiseEvent(ref _windowHeight, value);
		}

		public double WindowWidth
		{
			get => _windowWidth;
			set => SetAndRaiseEvent(ref _windowWidth, value);
		}

		public string ProcessPath { get; set; } = string.Empty;
		public double LeftOffset { get; set; }
		public double RightOffset { get; set; }
		public double TopOffset { get; set; }
		public double BottomOffset { get; set; }
		public double[] BoundaryOffset { get => new double[] { LeftOffset, RightOffset, TopOffset, BottomOffset }; }
	}
}
