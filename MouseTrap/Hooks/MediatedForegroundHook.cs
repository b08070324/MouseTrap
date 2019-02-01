using MouseTrap.Foundation;
using MouseTrap.Interop;
using MouseTrap.Models;

namespace MouseTrap.Hooks
{
	public class MediatedForegroundHook : ForegroundHook
	{
		private IMediator _mediator;

		public MediatedForegroundHook(IMediator mediator)
		{
			_mediator = mediator;
			_mediator.PropertyChanged += Mediator_PropertyChanged;
			ForegroundWindowChanged += ForegroundHook_ForegroundWindowChanged;
		}

		private void Mediator_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(IMediator.CurrentView):
					OnViewChanged();
					break;
				case nameof(IMediator.AppClosing):
					OnAppClosing();
					break;
			}
		}

		private void ForegroundHook_ForegroundWindowChanged(object sender, ForegroundHookEventArgs e)
		{
			_mediator.SetForegroundWindow(new WindowItem
			{
				Handle = e.Handle,
				ProcessId = e.WindowThreadProcId,
				ProcessPath = e.ProcessPath,
				Title = NativeMethods.GetWindowText(e.Handle)
			});
		}

		public void OnViewChanged()
		{
			if (_mediator.CurrentView == ViewType.LockWindow)
			{
				// Enable FG hook
				StartHook();
			}
			else
			{
				// Disable FG hook
				StopHook();
			}
		}

		public void OnAppClosing()
		{
			StopHook();
		}
	}
}
