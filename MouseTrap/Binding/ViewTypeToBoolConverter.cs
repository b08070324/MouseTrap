using System;
using System.Globalization;
using System.Windows.Data;
using MouseTrap.Models;

namespace MouseTrap.Binding
{
	public class ViewTypeToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var currentView = (ViewType)value;
			var targetViewType = (ViewType)parameter;
			return currentView == targetViewType;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
