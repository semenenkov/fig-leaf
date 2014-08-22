using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FigLeaf.Core
{
	public class AppConfigSettings : ISettings
	{
		public string SourceDir { get; set; }
		public string TargetDir { get; set; }
		public bool ExcludeFigLeafDir { get; set; }
		
		public string MasterPassword { get; set; }
		public PasswordRule PasswordRule { get; set; }
		
		public List<string> VideoExtensions { get; private set; }
		public int ThumbnailSize { get; set; }
		
		public bool DetailedLogging { get; set; }

		public string Culture { get; set; }

		public AppConfigSettings()
		{
			string exeFileName = string.IsNullOrEmpty(Console.Title)
				? Application.ExecutablePath
				: Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "FigLeaf.UI.exe");
			KeyValueConfigurationCollection appSettings = ConfigurationManager.OpenExeConfiguration(exeFileName).AppSettings.Settings;

			SourceDir = appSettings["SourceDir"].Value;
			TargetDir = appSettings["TargetDir"].Value;
			ExcludeFigLeafDir = bool.Parse(appSettings["ExcludeFigLeafDir"].Value);

			MasterPassword = appSettings["MasterPassword"].Value;
			PasswordRule = (PasswordRule)Enum.Parse(typeof(PasswordRule), appSettings["PasswordRule"].Value);

			VideoExtensions = appSettings["VideoExtensions"].Value.Split('|').ToList();
			ThumbnailSize = int.Parse(appSettings["ThumbnailSize"].Value);

			DetailedLogging = bool.Parse(appSettings["DetailedLogging"].Value);

			Culture = appSettings["Culture"].Value;
		}

		public void Save()
		{
			Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath); 
			
			config.AppSettings.Settings["SourceDir"].Value = SourceDir;
			config.AppSettings.Settings["TargetDir"].Value = TargetDir;
			config.AppSettings.Settings["ExcludeFigLeafDir"].Value = ExcludeFigLeafDir.ToString();
			config.AppSettings.Settings["MasterPassword"].Value = MasterPassword;
			config.AppSettings.Settings["PasswordRule"].Value = PasswordRule.ToString();
			config.AppSettings.Settings["ThumbnailSize"].Value = ThumbnailSize.ToString(CultureInfo.InvariantCulture);
			config.AppSettings.Settings["DetailedLogging"].Value = DetailedLogging.ToString();
			config.AppSettings.Settings["Culture"].Value = Culture;

			config.Save(ConfigurationSaveMode.Modified);
		}
	}
}
