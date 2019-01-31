namespace MouseTrap.Foundation
{
	public class MediatorEventLog
	{
		public MediatorEventLog(IMediator mediator)
		{
			mediator.OnWindowListUpdated += Mediator_OnWindowListUpdated;
			mediator.OnAppClosing += Mediator_OnAppClosing;
			mediator.OnBoundaryOffsetUpdated += Mediator_OnBoundaryOffsetUpdated;
			mediator.OnForegroundWindowUpdated += Mediator_OnForegroundWindowUpdated;
			mediator.OnTargetWindowUpdated += Mediator_OnTargetWindowUpdated;
			mediator.OnViewChanged += Mediator_OnViewChanged;
		}

		private void Mediator_OnViewChanged()
		{
			System.Diagnostics.Debug.WriteLine("Mediator_OnViewChanged");
		}

		private void Mediator_OnTargetWindowUpdated()
		{
			System.Diagnostics.Debug.WriteLine("Mediator_OnTargetWindowUpdated");
		}

		private void Mediator_OnForegroundWindowUpdated()
		{
			System.Diagnostics.Debug.WriteLine("Mediator_OnForegroundWindowUpdated");
		}

		private void Mediator_OnBoundaryOffsetUpdated()
		{
			System.Diagnostics.Debug.WriteLine("Mediator_OnBoundaryOffsetUpdated");
		}

		private void Mediator_OnAppClosing()
		{
			System.Diagnostics.Debug.WriteLine("Mediator_OnAppClosing");
		}

		private void Mediator_OnWindowListUpdated()
		{
			System.Diagnostics.Debug.WriteLine("Mediator_OnWindowListUpdated");
		}
	}
}
