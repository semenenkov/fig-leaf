using Antlr.Runtime;

namespace FigLeaf.Core.PasswordRules
{
	public class PasswordGenerator
	{
		private readonly ParserStream _expressionStream;
		private readonly string _masterPassword;

		public PasswordGenerator(PasswordRule passwordRule, string customRuleExpression, string masterPassword)
		{
			_masterPassword = masterPassword;

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
					break;
			}

			_expressionStream = new ParserStream(expression);
		}

		public string GetPassword(string fileName)
		{
			_expressionStream.Reset();
			var lexer = new FigLeafPasswordRuleLexer(_expressionStream);
			var tokenStream = new CommonTokenStream(lexer);
			var parser = new FigLeafPasswordRuleParser(tokenStream);
			return parser.functionArgument(fileName, _masterPassword);
		}
	}
}
