using MouseTrap.Foundation;
using MouseTrap.Interop;
using MouseTrap.Models;

namespace MouseTrap.Hooks
{
	public class MediatedMouseHook : MouseHook
	{
		private IMediator _mediator;

		public MediatedMouseHook(IMediator mediator)
		{
			_mediator = mediator;
			_mediator.PropertyChanged += Mediator_PropertyChanged;
			UpdateMouseHookMargin();
		}

		private void Mediator_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(IMediator.CurrentView):
					OnViewChanged();
					break;
				case nameof(IMediator.TargetWindow):
					UpdateMouseHookRegion();
					break;
				case nameof(IMediator.ForegroundWindow):
					OnForegroundWindowUpdated();
					break;
				case nameof(IMediator.BoundaryOffset):
					UpdateMouseHookMargin();
					break;
				case nameof(IMediator.AppClosing):
					OnAppClosing();
					break;
			}
		}

		private void UpdateMouseHookRegion()
		{
			if (_mediator.TargetWindow == null) return;

			var region = _mediator.TargetWindow.Dimensions;

			SetRegion(new Win32Rect
			{
				Left = (int)region.Left,
				Top = (int)region.Top,
				Right = (int)region.Right,
				Bottom = (int)region.Bottom
			});
		}

		private void UpdateMouseHookMargin()
		{
			var margin = _mediator.BoundaryOffset;

			SetMargin(new Win32Rect
			{
				Left = (int)margin.Left,
				Top = (int)margin.Top,
				Right = (int)-margin.Right,
				Bottom = (int)-margin.Bottom
			});
		}

		public void OnForegroundWindowUpdated()
		{
			// Check to start mouse hook
			if (_mediator.TargetWindow.IsValid)
			{
				if (_mediator.TargetWindow.IsMatch(_mediator.ForegroundWindow))
				{
					TrapMouse = true;
				}
				else
				{
					TrapMouse = false;
				}
			}
		}

		public void OnViewChanged()
		{
			if (_mediator.CurrentView == ViewType.LockWindow)
			{
				// Start mouse hook
				StartHook();
			}
			else
			{
				// Disable mouse hook
				StopHook();
			}
		}

		public void OnAppClosing()
		{
			StopHook();
		}
	}
}
