using MouseTrap.Data;
using MouseTrap.Models;
using MouseTrap.ViewModels;
using System;
using System.Windows;

namespace MouseTrap
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private IApplicationSystem ApplicationSystem { get; set; }
		private MainWindowViewModel MainWindowViewModel { get; set; }

		public App()
		{
			Startup += App_Startup;
			Exit += App_Exit;
		}

		private void App_Startup(object sender, StartupEventArgs e)
		{
			// Create system
			ApplicationSystem = ApplicationSystemFactory.GetApplicationSystem();

			// Create factories
			WindowListViewModel windowListLiveModelFactory() { return new WindowListLiveModel(new WindowCatalogue()); }
			FindProgramViewModel findProgramViewModelFactory() { return new FindProgramLiveModel(); }
			LockWindowViewModel lockWindowViewModelFactory() { return new LockWindowLiveModel(ApplicationSystem.TargetWindowDetails); }
			ToolBarViewModel toolBarViewModelFactory() { return new ToolBarLiveModel(); }

			// Create main view model
			MainWindowViewModel = new MainWindowLiveModel(
				ApplicationSystem, 
				windowListLiveModelFactory, 
				findProgramViewModelFactory,
				lockWindowViewModelFactory,
				toolBarViewModelFactory);

			// Show window
			var mainWindow = new MainWindow(MainWindowViewModel);
			mainWindow.Show();
		}

		private void App_Exit(object sender, ExitEventArgs e)
		{
			ApplicationSystem.ApplicationState.CancelWatch();
		}
	}
}
