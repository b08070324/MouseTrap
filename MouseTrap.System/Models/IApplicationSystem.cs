namespace MouseTrap.Models
{
	/// <summary>
	/// Aggregated interface to allow the UI to interact with the system
	/// </summary>
	public interface IApplicationSystem
	{
		IApplicationState ApplicationState { get; }
		ITargetWindowDetails TargetWindowDetails { get; }
	}
}
