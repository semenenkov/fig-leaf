using System.ComponentModel;

namespace FigLeaf.Core
{
    public enum PasswordRule
    {
		[Description("File name numbers followed by master password")]
		FileNameNumbersPlusPassword,
		[Description("Master password only")]
		Password,
		[Description("Master password followed by file name numbers")]
		PasswordPlusFileNameNumbers
    }
}
