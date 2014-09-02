using System;
using System.Windows;
using System.Windows.Data;
using FigLeaf.Core;

namespace FigLeaf.UI
{
	public class CustomRuleHeightConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if ((value != null) && ((PasswordRule)value) == PasswordRule.Custom)
				return new GridLength(1, GridUnitType.Star);

			return new GridLength(0, GridUnitType.Star);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
