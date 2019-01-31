using MouseTrap.Foundation;
using MouseTrap.Models;
using MouseTrap.ViewModels;

namespace MouseTrap.DesignModels
{
	public class WindowListDesignModel : WindowListViewModel
	{
		public WindowListDesignModel() : this(new DesignMediator())
		{
		}

		public WindowListDesignModel(IMediator mediator) : base(mediator)
		{
			mediator.SetTargetWindow(mediator.WindowList[4]);
		}
	}
}