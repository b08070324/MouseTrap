using System;
using MouseTrap.Foundation;
using MouseTrap.Models;

namespace MouseTrap.Hooks
{
	public class MediatedWindowClosedHook : WindowClosedHook
	{
		private IMediator _mediator;

		public MediatedWindowClosedHook(IMediator mediator)
		{
			_mediator = mediator;
			_mediator.PropertyChanged += Mediator_PropertyChanged;
			WindowClosed += UpdateHook_WindowClosed;
		}

		private void Mediator_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(IMediator.CurrentView):
					ToggleHookBasedOnValidity();
					break;
				case nameof(IMediator.ForegroundWindow):
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

		private void UpdateHook_WindowClosed(object sender, EventArgs e)
		{
			_mediator.TargetWindowLost();
		}
	}
}
