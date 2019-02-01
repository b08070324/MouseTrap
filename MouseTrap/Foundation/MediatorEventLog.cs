using static System.Diagnostics.Debug;

namespace MouseTrap.Foundation
{
	public class MediatorEventLog
	{
		public MediatorEventLog(IMediator mediator)
		{
			mediator.PropertyChanged += Mediator_PropertyChanged;
		}

		private void Mediator_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(IMediator.CurrentView):
					WriteLine("Mediator_OnViewChanged");
					break;
				case nameof(IMediator.TargetWindow):
					WriteLine("Mediator_OnTargetWindowUpdated");
					break;
				case nameof(IMediator.ForegroundWindow):
					WriteLine("Mediator_OnForegroundWindowUpdated");
					break;
				case nameof(IMediator.BoundaryOffset):
					WriteLine("Mediator_OnBoundaryOffsetUpdated");
					break;
				case nameof(IMediator.WindowList):
					WriteLine("Mediator_OnWindowListUpdated");
					break;
				case nameof(IMediator.AppClosing):
					WriteLine("Mediator_OnAppClosing");
					break;
			}
		}
	}
}
