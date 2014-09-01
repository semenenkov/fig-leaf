using Antlr.Runtime;

namespace FigLeaf.Core.PasswordRules
{
	public class Parser
	{
		private readonly ParserStream _expressionStream;

		public Parser(string expression)
		{
			_expressionStream = new ParserStream(expression);
		}

		public string GetPasswordRule(string fileName, string password)
		{
			_expressionStream.Reset();
			var lexer = new FigLeafPasswordRuleLexer(_expressionStream);
			var tokenStream = new CommonTokenStream(lexer);
			var parser = new FigLeafPasswordRuleParser(tokenStream);
			return parser.functionArgument(fileName, password);
		}
	}
}
