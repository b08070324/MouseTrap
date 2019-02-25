using MouseTrap.Behaviours;
using MouseTrap.Hooks;
using MouseTrap.Models;

namespace MouseTrap
{
	public static class ApplicationSystemFactory
	{
		/// <summary>
		/// Configures an instance of IApplicationSystem with appropriate services and behaviours
		/// </summary>
		/// <returns>The instace as IApplicationSystem suitable for use by the user interface</returns>
		public static IApplicationSystem GetApplicationSystem()
		{
			var ApplicationState = new ApplicationState();
			var TargetWindowDetails = new TargetWindowDetails();
			var ForegroundWindowHook = new ForegroundWindowHook();
			var WindowUpdateHook = new WindowUpdateHook();
			var MouseHook = new MouseHook();

			return new ApplicationSystem
			{
				ApplicationState = ApplicationState,
				TargetWindowDetails = TargetWindowDetails,
				ForegroundWindowHook = ForegroundWindowHook,
				WindowUpdateHook = WindowUpdateHook,
				MouseHook = MouseHook,
				SpecificWindowBehaviour = new SpecificWindowBehaviour(ApplicationState, ForegroundWindowHook, WindowUpdateHook, MouseHook),
				ProgramPathBehaviour = new ProgramPathBehaviour(ApplicationState, ForegroundWindowHook),
				ProgramPathToSpecificWindowBehaviour = new ProgramPathToSpecificWindowBehaviour(ApplicationState, ForegroundWindowHook),
				WatchCancelledBehaviour = new WatchCancelledBehaviour(ApplicationState, ForegroundWindowHook, WindowUpdateHook, MouseHook),
				WindowClosedBehaviour = new WindowClosedBehaviour(ApplicationState, WindowUpdateHook),
				WindowFocusBehaviour = new WindowFocusBehaviour(ApplicationState, ForegroundWindowHook, MouseHook),
				WindowSizeBehaviour = new WindowSizeBehaviour(ApplicationState, WindowUpdateHook, MouseHook),
				PaddingChangeBehaviour = new PaddingChangeBehaviour(ApplicationState, MouseHook),
				UpdateDetailsBehaviour = new UpdateDetailsBehaviour(ApplicationState, TargetWindowDetails, WindowUpdateHook, ForegroundWindowHook)
			};
		}

		/// <summary>
		/// Specific implementation of IApplicationSystem used by ApplicationSystemFactory
		/// </summary>
		private class ApplicationSystem : IApplicationSystem
		{
			public IApplicationState ApplicationState { get; set; }
			public ITargetWindowDetails TargetWindowDetails { get; set; }
			public IForegroundWindowHook ForegroundWindowHook { get; set; }
			public IWindowUpdateHook WindowUpdateHook { get; set; }
			public IMouseHook MouseHook { get; set; }
			public SpecificWindowBehaviour SpecificWindowBehaviour { get; set; }
			public ProgramPathBehaviour ProgramPathBehaviour { get; set; }
			public ProgramPathToSpecificWindowBehaviour ProgramPathToSpecificWindowBehaviour { get; set; }
			public WatchCancelledBehaviour WatchCancelledBehaviour { get; set; }
			public WindowClosedBehaviour WindowClosedBehaviour { get; set; }
			public WindowFocusBehaviour WindowFocusBehaviour { get; set; }
			public WindowSizeBehaviour WindowSizeBehaviour { get; set; }
			public PaddingChangeBehaviour PaddingChangeBehaviour { get; set; }
			public UpdateDetailsBehaviour UpdateDetailsBehaviour { get; set; }
		}
	}
}
