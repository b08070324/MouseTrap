using MouseTrap.Binding;
using MouseTrap.Foundation;
using MouseTrap.Models;
using MouseTrap.ViewModels;

namespace MouseTrap.DesignModels
{
	public class FindProgramDesignModel : FindProgramViewModel
	{
		public FindProgramDesignModel() : this(new DesignMediator()) { }
		public FindProgramDesignModel(IMediator mediator) : base(mediator) { }
	}
}
