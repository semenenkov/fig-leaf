using System.ComponentModel;

namespace FigLeaf.Core
{
    public enum PasswordRule
    {
		[Description("Core_PasswordRule_Password")]
		Password,
		[Description("Core_PasswordRule_PasswordPlusFileNameNumbers")]
		PasswordPlusFileNameNumbers,
		[Description("Core_PasswordRule_FileNameNumbersPlusPassword")]
		FileNameNumbersPlusPassword,
		[Description("Core_PasswordRule_Custom")]
		Custom,
	}
}
