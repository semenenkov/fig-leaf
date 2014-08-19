
using System.Windows;
using System.Windows.Controls;

namespace FigLeaf.UI
{
	public sealed class CancelButton : Button
	{
		static CancelButton()
		{
			IsEnabledProperty.OverrideMetadata
			(
				typeof(CancelButton),
				new FrameworkPropertyMetadata
				(
					true,
					null,
					CoerceIsEnabled
				)
			);
		}

		private static object CoerceIsEnabled(DependencyObject source, object value)
		{
			return value;
		}
	}
}
