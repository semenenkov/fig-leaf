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

		public Parser(PasswordRule passwordRule, string customRuleExpression)
		{
			string expression = null;
			switch (passwordRule)
			{
				case PasswordRule.FileNameNumbersPlusPassword:
					expression = "Add(Digits(RemoveFileExtension(FileName)), Password)";
					break;
				case PasswordRule.Password:
					expression = "Password";
					break;
				case PasswordRule.PasswordPlusFileNameNumbers:
					expression = "Add(Password, Digits(RemoveFileExtension(FileName)))";
					break;
				default:
					expression = customRuleExpression;
			}

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
