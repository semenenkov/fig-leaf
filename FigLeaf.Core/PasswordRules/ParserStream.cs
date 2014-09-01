using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace FigLeaf.Core.PasswordRules
{
	public class ParserStream : ANTLRStringStream
	{
		public ParserStream(string input)
		{
			// TODO process l10n
			data = input.ToCharArray();
			n = data.Length;
		}
	}
}
