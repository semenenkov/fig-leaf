using System;
using Antlr.Runtime;

namespace FigLeaf.Core.PasswordRules
{
	public class PasswordGenerator
	{
		private readonly string _masterPassword;
		private readonly string _expression;

		public PasswordGenerator(PasswordRule passwordRule, string customRuleExpression, string masterPassword)
		{
			_masterPassword = masterPassword;

			switch (passwordRule)
			{
				case PasswordRule.FileNameNumbersPlusPassword:
					_expression = "Add(Digits(RemoveFileExtension(FileName)), Password)";
					break;
				case PasswordRule.Password:
					_expression = "Password";
					break;
				case PasswordRule.PasswordPlusFileNameNumbers:
					_expression = "Add(Password, Digits(RemoveFileExtension(FileName)))";
					break;
				default:
					_expression = customRuleExpression;
					break;
			}

			// test expression syntax
			GetPassword("test123.jpg", true);
		}

		public string GetPassword(string fileName, bool isWarmup = false)
		{
			try
			{
				var expressionStream = new ParserStream(_expression);
				var lexer = new FigLeafPasswordRuleLexer(expressionStream);
				var tokenStream = new CommonTokenStream(lexer);
				var parser = new FigLeafPasswordRuleParser(tokenStream);
				return parser.functionArgument(fileName, _masterPassword);
			}
			catch (RecognitionException)
			{
				string message = isWarmup
					? Properties.Resources.Core_PasswordRule_BadFormula
					: string.Format(Properties.Resources.Core_PasswordRule_BadFormulaFormat, fileName);
				throw new ApplicationException(message);
			}
		}
	}
}
