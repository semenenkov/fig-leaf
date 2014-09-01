using FigLeaf.Core.PasswordRules;
using NUnit.Framework;

namespace FigLeaf.Core.UnitTest.PasswordRules
{
	[TestFixture]
    public class ParserTests
    {
		[Test]
		[TestCase("FileName", "test123.jpg", "Pwd01", "test123.jpg")]
		[TestCase("Password", "test123.jpg", "Pwd01", "Pwd01")]
		[TestCase("Reverse(Password)", "test123.jpg", "Pwd01", "10dwP")]
		[TestCase("Upper(Password)", "test123.jpg", "Pwd01", "PWD01")]
		[TestCase("Lower(Password)", "test123.jpg", "Pwd01", "pwd01")]
		[TestCase("Digits(Password)", "test123.jpg", "Pwd01", "01")]
		[TestCase("Digits(FileName)", "test123.jpg", "Pwd01", "123")]
		[TestCase("RemoveFileExtension(FileName)", "test123.jpg", "Pwd01", "test123")]
		[TestCase("FileExtension(FileName)", "test123.jpg", "Pwd01", "jpg")]
		[TestCase("Len(FileName)", "test123.jpg", "Pwd01", "11")]
		[TestCase("Len(Password)", "test123.jpg", "Pwd01", "5")]
		[TestCase("Add(FileName,Password)", "test123.jpg", "Pwd01", "test123.jpgPwd01")]
		[TestCase("Left(FileName,4)", "test123.jpg", "Pwd01", "test")]
		[TestCase("Left(FileName,40)", "test123.jpg", "Pwd01", "test123.jpg")]
		[TestCase("Right(FileName, 4)", "test123.jpg", "Pwd01", ".jpg")]
		public void TestAllFunctions(string expression, string arg1, string arg2, string result)
		{
			var parser = new Parser(expression);
			Assert.AreEqual(result, parser.GetPasswordRule(arg1, arg2));
		}

		[Test]
		public void TestInstanceReuse()
		{
			const string expression = "Add(FileName,Password)";
			var parser = new Parser(expression);
			Assert.AreEqual("ab", parser.GetPasswordRule("a", "b"));
			Assert.AreEqual("cd", parser.GetPasswordRule("c", "d"));
		}
    }
}
