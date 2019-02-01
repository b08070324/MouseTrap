using MouseTrap.Hooks;
using MouseTrap.WindowQuery;

namespace MouseTrap.Foundation
{
	public static class ConcreteMediatorFactory
	{
		private static IMediator mediator;
		private static MediatedForegroundHook foregroundHook;
		private static MediatedWindowUpdateHook windowUpdateHook;
		private static MediatedWindowClosedHook windowClosedHook;
		private static MediatedMouseHook mouseHook;
		//private static MediatorEventLog log;

		public static IMediator GetMediator()
		{
			if (mediator == null)
			{
				mediator = new ConcreteMediator();
				//log = new MediatorEventLog(mediator);
				mediator.SetApplicationState(new ApplicationState(mediator));
				mediator.SetWindowQueryManager(new WindowQueryManager());
				foregroundHook = new MediatedForegroundHook(mediator);
				windowUpdateHook = new MediatedWindowUpdateHook(mediator);
				windowClosedHook = new MediatedWindowClosedHook(mediator);
				mouseHook = new MediatedMouseHook(mediator);
			}

			return mediator;
		}
	}
}
