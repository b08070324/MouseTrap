using MouseTrap.Foundation;
using MouseTrap.Models;
using MouseTrap.ViewModels;

namespace MouseTrap.DesignModels
{
	public class LockWindowDesignModel : LockWindowViewModel
	{
		public LockWindowDesignModel() : this(new DesignMediator())
		{
		}

		public LockWindowDesignModel(IMediator mediator) : base(mediator)
		{
			mediator.SetTargetWindow(mediator.WindowList[0]);
			mediator.SetForegroundWindow(mediator.WindowList[0]);
			mediator.SetCurrentView(ViewType.LockWindow);
		}
	}
}
