using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
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

		public AppConfigSettings()
		{
			NameValueCollection appSettings = ConfigurationManager.AppSettings;

			SourceDir = appSettings["SourceDir"];
			TargetDir = appSettings["TargetDir"];
			ExcludeFigLeafDir = bool.Parse(appSettings["ExcludeFigLeafDir"]);
			
			MasterPassword = appSettings["MasterPassword"];
			PasswordRule = (PasswordRule)Enum.Parse(typeof(PasswordRule), appSettings["PasswordRule"]);

			VideoExtensions = appSettings["VideoExtensions"].Split('|').ToList();
			ThumbnailSize = int.Parse(appSettings["ThumbnailSize"]);

			DetailedLogging = bool.Parse(appSettings["DetailedLogging"]);
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

			config.Save(ConfigurationSaveMode.Modified);
		}
	}
}
