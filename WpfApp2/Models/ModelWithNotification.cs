using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp2.Models
{
	public abstract class ModelWithNotification : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
		{
			field = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
