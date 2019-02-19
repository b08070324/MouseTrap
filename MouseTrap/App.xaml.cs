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
		private MainWindowViewModel MainWindowViewModel { get; set; }

		public App()
		{
			Startup += App_Startup;
			Exit += App_Exit;
		}

		private void App_Startup(object sender, StartupEventArgs e)
		{
			MainWindowViewModel = new MainWindowLiveModel();
			var mainWindow = new MainWindow(MainWindowViewModel);
			mainWindow.Show();
		}

		private void App_Exit(object sender, ExitEventArgs e)
		{
		}
	}
}
