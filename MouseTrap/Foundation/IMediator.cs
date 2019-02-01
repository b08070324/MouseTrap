using MouseTrap.Models;
using MouseTrap.WindowQuery;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MouseTrap.Foundation
{
	public interface IMediator : INotifyPropertyChanged
	{
		// Initialisation
		void SetApplicationState(ApplicationState applicationState);
		void SetWindowQueryManager(IWindowQueryManager queryManager);

		// Queries
		ObservableCollection<IWindowItem> WindowList { get; }
		ViewType CurrentView { get; }
		IWindowItem TargetWindow { get; }
		IWindowItem ForegroundWindow { get; }
		Dimensions BoundaryOffset { get; }
		bool IsTargetWindowFocused { get; }
		bool IsLockEnabled { get; }
		bool DoesWindowExist(IWindowItem windowItem);
		ObservableCollection<IWindowItem> GetWindowListUpdate();
		WindowItemUpdateDetails GetWindowItemUpdate(IWindowItem windowItem);

		// Commands
		void RefreshWindowList();
		void RefreshWindowDetails();
		void SetBoundaryOffset(Dimensions value);
		void SetCurrentView(ViewType viewType);
		void SetForegroundWindow(IWindowItem windowItem);
		void SetTargetWindow(IWindowItem windowItem);
		void SetTargetWindowTitle(string title);
		void SetTargetWindowRect(Dimensions value);
		void TargetWindowLost();
		void AppClosing();
	}
}
