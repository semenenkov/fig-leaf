using System.ComponentModel;

namespace FigLeaf.Core
{
	public enum ArchiveNameRule
	{
		[Description("Core_ArchiveNameRule_AddZipExtension")]
		AddZipExtension,
		[Description("Core_ArchiveNameRule_KeepOriginal")]
		KeepOriginal,
	}
}
