using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MouseTrap.Foundation
{
	public abstract class NotifyingObject : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// The name sent to PropertyChangedEventHandler needs to match the property name
		// This uses CallerMemberName to set this automatically when called from a property
		protected bool SetAndRaiseEvent<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
		{
			if (!EqualityComparer<T>.Default.Equals(field, value))
			{
				field = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
				return true;
			}

			return false;
		}

		protected void RaiseEvent([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
