using System.Globalization;
using FigLeaf.Core.Properties;

namespace FigLeaf.Core
{
	public static class Utils
	{
		public static void SetupCulture(Settings settings)
		{
			try
			{
				Resources.Culture = CultureInfo.GetCultureInfo(settings.Culture);
			}
			catch
			{
				settings.Culture = Resources.Common_Culture;
				Resources.Culture = CultureInfo.GetCultureInfo(settings.Culture);
			}
		}
	}
}
