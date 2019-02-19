using MouseTrap.Foundation;

namespace MouseTrap.ViewModels
{
	public abstract class LockWindowViewModel : NotifyingObject, IViewModel
	{
		private string _title = string.Empty;
		private bool _windowIsFocused;
		private double _windowHeight;
		private double _windowWidth;
		private double _leftOffset;
		private double _rightOffset;
		private double _topOffset;
		private double _bottomOffset;

		public string ProcessPath { get; set; } = string.Empty;

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

		public double LeftOffset
		{
			get => _leftOffset;
			set { SetAndRaiseEvent(ref _leftOffset, value); RaiseEvent(nameof(BoundaryOffset)); }
		}

		public double RightOffset
		{
			get => _rightOffset;
			set { SetAndRaiseEvent(ref _rightOffset, value); RaiseEvent(nameof(BoundaryOffset)); }
		}

		public double TopOffset
		{
			get => _topOffset;
			set { SetAndRaiseEvent(ref _topOffset, value); RaiseEvent(nameof(BoundaryOffset)); }
		}

		public double BottomOffset
		{
			get => _bottomOffset;
			set { SetAndRaiseEvent(ref _bottomOffset, value); RaiseEvent(nameof(BoundaryOffset)); }
		}

		// Derived property used by marginBox
		public double[] BoundaryOffset { get => new double[] { LeftOffset, RightOffset, TopOffset, BottomOffset }; }
	}
}
