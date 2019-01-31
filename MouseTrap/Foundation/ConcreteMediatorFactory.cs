using MouseTrap.Hooks;
using MouseTrap.WindowQuery;

namespace MouseTrap.Foundation
{
	public static class ConcreteMediatorFactory
	{
		private static IMediator mediator;
		private static HookManager hookManager;
		//private static MediatorEventLog log;

		public static IMediator GetMediator()
		{
			if (mediator == null)
			{
				mediator = new ConcreteMediator();
				//log = new MediatorEventLog(mediator);
				mediator.SetApplicationState(new ApplicationState(mediator));
				mediator.SetWindowQueryManager(new WindowQueryManager());
				hookManager = new HookManager(mediator);
			}

			return mediator;
		}
	}
}
