using System.ComponentModel;
using System.Windows;
using MouseTrap.Foundation;
using MouseTrap.Models;

namespace MouseTrap.ViewModels
{
	public class LockWindowViewModel : BaseViewModel
	{
		private string _title = "<No Target Window>";
		private string _processPath = "-";
		private double _windowWidth;
		private double _windowHeight;
		private bool _windowIsFocused;

		private double _leftOffset;
		private double _topOffset;
		private double _rightOffset;
		private double _bottomOffset;

		public LockWindowViewModel(IMediator mediator) : base(mediator)
		{
			mediator.OnForegroundWindowUpdated += OnForegroundWindowUpdated;
			mediator.OnViewChanged += Mediator_OnViewChanged;
			mediator.OnTargetWindowUpdated += Mediator_OnTargetWindowUpdated;
			mediator.OnBoundaryOffsetUpdated += Mediator_OnBoundaryOffsetUpdated;
			PropertyChanged += LockWindowViewModel_PropertyChanged;
		}

		private void LockWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var name = e.PropertyName;
			if (name == nameof(LeftOffset) ||
				name == nameof(TopOffset) ||
				name == nameof(RightOffset) ||
				name == nameof(BottomOffset))
			{
				_mediator.SetBoundaryOffset(new Dimensions(_leftOffset, _topOffset, _rightOffset, _bottomOffset));
			}
		}

		public void OnForegroundWindowUpdated()
		{
			WindowIsFocused = _mediator.IsTargetWindowFocused;
		}

		private void Mediator_OnViewChanged()
		{
			if (_mediator.CurrentView == ViewType.LockWindow)
			{
				BoundaryUpdated();
				TargetWindowUpdated();
			}
		}

		private void Mediator_OnTargetWindowUpdated()
		{
			if (_mediator.CurrentView == ViewType.LockWindow)
			{
				TargetWindowUpdated();
			}
		}

		private void Mediator_OnBoundaryOffsetUpdated()
		{
			if (_mediator.CurrentView == ViewType.LockWindow)
			{
				BoundaryUpdated();
			}
		}

		private void BoundaryUpdated()
		{
			var boundary = _mediator.BoundaryOffset;
			_leftOffset = boundary.Left;
			_topOffset = boundary.Top;
			_rightOffset = boundary.Right;
			_bottomOffset = boundary.Bottom;
			RaiseEvent(nameof(BoundaryOffset));
		}

		private void TargetWindowUpdated()
		{
			if (_mediator.TargetWindow == null)
			{
				Title = "<No Target Window>";
				ProcessPath = "-";
				WindowWidth = 0;
				WindowHeight = 0;
			}
			else if (_mediator.TargetWindow.IsValid)
			{
				Title = _mediator.TargetWindow.Title;
				ProcessPath = _mediator.TargetWindow.ProcessPath;
				WindowWidth = _mediator.TargetWindow.Width;
				WindowHeight = _mediator.TargetWindow.Height;
			}
			else
			{
				Title = "<Waiting for window>";
				ProcessPath = _mediator.TargetWindow.ProcessPath;
				WindowWidth = 0;
				WindowHeight = 0;
			}
		}

		public double WindowWidth
		{
			get => _windowWidth;
			protected set => SetAndRaiseEvent(ref _windowWidth, value);
		}

		public double WindowHeight
		{
			get => _windowHeight;
			protected set => SetAndRaiseEvent(ref _windowHeight, value);
		}

		public bool WindowIsFocused
		{
			get => _windowIsFocused;
			protected set => SetAndRaiseEvent(ref _windowIsFocused, value);
		}

		public string Title
		{
			get => _title;
			protected set => SetAndRaiseEvent(ref _title, value);
		}

		public string ProcessPath
		{
			get => _processPath;
			protected set => SetAndRaiseEvent(ref _processPath, value);
		}

		public Dimensions BoundaryOffset
		{
			get => _mediator.BoundaryOffset;
		}

		public double LeftOffset
		{
			get => _leftOffset;
			set => SetAndRaiseEvent(ref _leftOffset, value);
		}

		public double TopOffset
		{
			get => _topOffset;
			set => SetAndRaiseEvent(ref _topOffset, value);
		}

		public double RightOffset
		{
			get => _rightOffset;
			set => SetAndRaiseEvent(ref _rightOffset, value);
		}

		public double BottomOffset
		{
			get => _bottomOffset;
			set => SetAndRaiseEvent(ref _bottomOffset, value);
		}
	}
}
