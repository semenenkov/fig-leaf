using Antlr.Runtime;

namespace FigLeaf.Core.PasswordRules
{
	public class ParserStream : ANTLRStringStream
	{
		public ParserStream(string input)
		{
			// TODO process formula l10n if needed (not required yet)
			data = input.ToCharArray();
			n = data.Length;
		}
	}
}
