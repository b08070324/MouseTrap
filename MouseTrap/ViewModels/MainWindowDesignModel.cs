namespace MouseTrap.ViewModels
{
	public class MainWindowDesignModel : MainWindowViewModel
	{
		public MainWindowDesignModel()
		{
			ToolBarViewModel = new ToolBarDesignModel();
			CurrentViewModel = new WindowListDesignModel();
		}
	}
}
