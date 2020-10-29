using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AutobotLauncher.Utils.Converters
{
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class BoolToVisibilityConverter : IValueConverter
	{
		public Visibility NeutralValue { get; set; }
		public Visibility TrueValue { get; set; }
		public Visibility FalseValue { get; set; }

		public BoolToVisibilityConverter()
		{
			// set defaults
			FalseValue = Visibility.Hidden;
			TrueValue = Visibility.Visible;
			NeutralValue = Visibility.Collapsed;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null ? 
				NeutralValue : 
				(bool) value ? TrueValue : FalseValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
