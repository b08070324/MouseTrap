using MouseTrap.Models;
using System;

namespace MouseTrap.Data
{
	public interface IWindowCatalogue
	{
		void EnumerateWindows(Action<IWindow> callback);
	}
}
