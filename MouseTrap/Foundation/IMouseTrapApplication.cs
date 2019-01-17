using System.Collections.Generic;

namespace MouseTrap.Foundation
{
	interface IMouseTrapApplication
	{
		ICollection<WindowItem> GetWindowList();
		UpdatedWindowDetails UpdateWindowDetails();
		void SetMouseHookState(bool mouseTrapRequested);
		void SelectWindow(WindowItem windowItem);
		bool IsWindowSelected { get; }
	}
}
