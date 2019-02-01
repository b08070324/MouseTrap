using System;
using MouseTrap.Foundation;
using MouseTrap.Models;

namespace MouseTrap.Hooks
{
	public class MediatedWindowUpdateHook : WindowUpdateHook
	{
		private IMediator _mediator;

		public MediatedWindowUpdateHook(IMediator mediator)
		{
			_mediator = mediator;
			_mediator.PropertyChanged += Mediator_PropertyChanged;
			WindowTitleChanged += UpdateHook_WindowTitleChanged;
			WindowRectChanged += UpdateHook_WindowRectChanged;
		}

		private void Mediator_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(IMediator.CurrentView):
					ToggleHookBasedOnValidity();
					break;
				case nameof(IMediator.TargetWindow):
					ToggleHookBasedOnValidity();
					break;
				case nameof(IMediator.AppClosing):
					OnAppClosing();
					break;
			}
		}

		public void ToggleHookBasedOnValidity()
		{
			if (_mediator.CurrentView == ViewType.LockWindow)
			{
				// Enable window update hook
				if (_mediator.TargetWindow != null && _mediator.TargetWindow.IsValid)
				{
					StartHook(_mediator.TargetWindow.Handle);
				}
			}
			else
			{
				// Disable window update hook
				StopHook();
			}
		}

		public void OnAppClosing()
		{
			StopHook();
		}

		private void UpdateHook_WindowTitleChanged(object sender, WindowTitleEventArgs e)
		{
			_mediator.SetTargetWindowTitle(e.Title);
		}

		private void UpdateHook_WindowRectChanged(object sender, WindowRectEventArgs e)
		{
			var dimensions = new Dimensions(e.WindowRect.Left, e.WindowRect.Top, e.WindowRect.Right, e.WindowRect.Bottom);
			_mediator.SetTargetWindowRect(dimensions);
		}
	}
}
