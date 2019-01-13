using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace WpfApp2.Data
{
	public class BoolToValueConverter<T> : IValueConverter
	{
		public T TrueValue { get; set; }
		public T FalseValue { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool boolValue) return boolValue ? TrueValue : FalseValue;
			return FalseValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null && EqualityComparer<T>.Default.Equals((T)value, TrueValue);
		}
	}

	public class BoolToStringConverter : BoolToValueConverter<string> { }
}
