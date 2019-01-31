using MouseTrap.Foundation;
using MouseTrap.Models;
using MouseTrap.ViewModels;

namespace MouseTrap.DesignModels
{
	public class MainWindowDesignModel
	{
		public ToolBarViewModel ToolBarViewModel { get; set; }
		public BaseViewModel CurrentViewModel { get; set; }

		public MainWindowDesignModel()
		{
			IMediator mediator = new DesignMediator();
			ToolBarViewModel = new ToolBarDesignModel(mediator);
			CurrentViewModel = new FindProgramDesignModel(mediator);
			mediator.SetCurrentView(ViewType.FindProgram);
			//CurrentViewModel = new WindowListDesignModel(mediator);
			//mediator.SetCurrentView(ViewType.WindowList);
			//CurrentViewModel = new LockWindowDesignModel(mediator);
			//mediator.SetCurrentView(ViewType.LockWindow);
		}
	}
}
