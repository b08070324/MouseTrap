using MouseTrap.Foundation;
using MouseTrap.Models;
using System.Windows;

namespace MouseTrap
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			this.Exit += App_Exit;
		}

		private void App_Exit(object sender, ExitEventArgs e)
		{
		}
	}
}
