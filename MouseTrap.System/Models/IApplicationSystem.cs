namespace MouseTrap.Models
{
	public interface IApplicationSystem
	{
		IApplicationState ApplicationState { get; }
		ITargetWindowDetails TargetWindowDetails { get; }
	}
}
