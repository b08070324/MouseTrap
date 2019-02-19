using MouseTrap.Data;
using MouseTrap.Models;
using MouseTrap.ViewModels;
using System.Windows;

namespace MouseTrap
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private IApplicationSystem ApplicationSystem { get; set; }

		public App()
		{
			Startup += App_Startup;
			Exit += App_Exit;
		}

		private void App_Startup(object sender, StartupEventArgs e)
		{
			// Show window
			var mainWindow = new MainWindow();
			mainWindow.Show();

			// Create system
			ApplicationSystem = ApplicationSystemFactory.GetApplicationSystem();

			// Initialise padding
			ApplicationSystem.ApplicationState.SetPadding(8, 8, 8, 8);

			// Create factories
			WindowListViewModel windowListLiveModelFactory() { return new WindowListLiveModel(new WindowCatalogue()); }
			FindProgramViewModel findProgramViewModelFactory() { return new FindProgramLiveModel(); }
			LockWindowViewModel lockWindowViewModelFactory() { return new LockWindowLiveModel(ApplicationSystem.ApplicationState, ApplicationSystem.TargetWindowDetails); }
			ToolBarViewModel toolBarViewModelFactory() { return new ToolBarLiveModel(ApplicationSystem.ApplicationState); }

			// Create main view model
			mainWindow.DataContext = new MainWindowLiveModel(
				ApplicationSystem, 
				windowListLiveModelFactory, 
				findProgramViewModelFactory,
				lockWindowViewModelFactory,
				toolBarViewModelFactory);
		}

		private void App_Exit(object sender, ExitEventArgs e)
		{
			ApplicationSystem.ApplicationState.CancelWatch();
		}
	}
}
