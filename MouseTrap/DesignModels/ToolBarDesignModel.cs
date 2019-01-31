using MouseTrap.Foundation;
using MouseTrap.Models;
using MouseTrap.ViewModels;

namespace MouseTrap.DesignModels
{
	public class ToolBarDesignModel : ToolBarViewModel
	{
		public ToolBarDesignModel() : this(new DesignMediator())
		{
		}

		public ToolBarDesignModel(IMediator mediator) : base(mediator)
		{
			mediator.SetCurrentView(ViewType.WindowList);
			//mediator.SetTargetWindow(mediator.WindowList[0]);
		}
	}
}
