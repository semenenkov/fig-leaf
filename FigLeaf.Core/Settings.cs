using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace FigLeaf.Core
{
	[DataContract(Namespace = Settings.Namespace)]
	public class Settings
	{
		private const string FileName = "FigLeaf.Settings.xml";
		private const string DefaultVideoExts = "3gp|avi|flv|m4v|mkv|mov|mp4|mpeg|mpg|mts|wmv";

		public const string Namespace = "https://github.com/semenenkov/fig-leaf/2014/08/26";

		[DataMember]
		public List<DirPair> Dirs { get; set; }
		[DataMember]
		public bool HasMultipleDirs { get; set; }
		[DataMember]
		public bool ExcludeFigLeafDir { get; set; }

		[DataMember]
		public string MasterPassword { get; set; }
		[DataMember]
		public PasswordRule PasswordRule { get; set; }

		[DataMember]
		public List<string> VideoExtensions { get; set; }
		[DataMember]
		public int ThumbnailSize { get; set; }

		[DataMember]
		public bool DetailedLogging { get; set; }

		[DataMember]
		public string Culture { get; set; }

		public Settings()
		{
			HasMultipleDirs = false;
			ExcludeFigLeafDir = true;
			PasswordRule = PasswordRule.FileNameNumbersPlusPassword;
			ThumbnailSize = 128;
			DetailedLogging = false;
			Culture = "en-US";

			CheckDefaults(this);
		}

		public void SaveToFile()
		{
			var ser = new DataContractSerializer(typeof(Settings));
			var xmlSettings = new XmlWriterSettings { Indent = true };
			using (var xmlWriter = XmlWriter.Create(FileName, xmlSettings))
				ser.WriteObject(xmlWriter, this);
		}

		public static Settings ReadFromFile(bool createOnError)
		{
			try
			{
				using (var fs = new FileStream(FileName, FileMode.Open))
				{
					var ser = new DataContractSerializer(typeof(Settings));
					var result = (Settings)ser.ReadObject(fs);
					CheckDefaults(result);
					return result;
				}
			}
			catch
			{
				if (createOnError)
					return new Settings();
			}

			return null;
		}

		private static void CheckDefaults(Settings result)
		{
			if (result.Dirs == null)
				result.Dirs = new List<DirPair>();
			if (result.Dirs.Count == 0)
				result.Dirs.Add(new DirPair(null, null));
			if (result.VideoExtensions == null)
				result.VideoExtensions = DefaultVideoExts.Split('|').ToList();
		}
	}
}
