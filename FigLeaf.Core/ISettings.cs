using System.Collections.Generic;

namespace FigLeaf.Core
{
	public interface ISettings
	{
		string SourceDir { get; set; }
		string TargetDir { get; set; }
		bool ExcludeFigLeafDir { get; set; }

		string MasterPassword { get; set; }
		PasswordRule PasswordRule { get; set; }

		List<string> VideoExtensions { get; }
		int ThumbnailSize { get; set; }

		bool DetailedLogging { get; set; }

		string Culture { get; set; }

		void Save();
	}
}
