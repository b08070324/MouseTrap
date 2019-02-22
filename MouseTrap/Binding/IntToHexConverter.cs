using System;
using System.Globalization;
using System.Windows.Data;

namespace MouseTrap.Binding
{
	public class IntToHexConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			IntPtr number = (IntPtr)value;
			return number.ToInt32().ToString("X8");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
