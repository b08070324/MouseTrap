using MouseTrap.WindowQuery;

namespace MouseTrap.Foundation
{
	public class DesignMediator : ConcreteMediator
	{
		public DesignMediator() : base()
		{
			SetApplicationState(new ApplicationState(this));
			SetWindowQueryManager(new DesignWindowQueryManager());
			RefreshWindowList();
		}
	}
}
