using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wpf.Example.Controls.Converter
{
	/// <ChangeLog>
	/// <Create Datum="26.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class LevelToIndentConverter : IValueConverter
	{
		// Wie viele Pixel soll pro Ebene eingerückt werden?
		public double IndentMultiplier { get; set; } = 19;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int level)
			{
				return new Thickness(level * IndentMultiplier, 0, 0, 0);
			}
			return new Thickness(0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	/// <ChangeLog>
	/// <Create Datum="26.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value is bool hasChildren && hasChildren) ? Visibility.Visible : Visibility.Hidden;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
	}
}