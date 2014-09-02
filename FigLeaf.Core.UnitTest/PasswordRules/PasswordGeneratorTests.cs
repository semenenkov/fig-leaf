using FigLeaf.Core.PasswordRules;
using NUnit.Framework;

namespace FigLeaf.Core.UnitTest.PasswordRules
{
	[TestFixture]
    public class PasswordGeneratorTests
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
		public void TestAllFunctions(string expression, string fileName, string password, string result)
		{
			var parser = new PasswordGenerator(PasswordRule.Custom, expression, password);
			Assert.AreEqual(result, parser.GetPassword(fileName));
		}

		[Test]
		public void TestInstanceReuse()
		{
			const string expression = "Add(FileName,Password)";
			var parser = new PasswordGenerator(PasswordRule.Custom, expression, "1");
			Assert.AreEqual("a1", parser.GetPassword("a"));
			Assert.AreEqual("b1", parser.GetPassword("b"));
		}

		[Test]
		[TestCase(PasswordRule.Password, "test123.jpg", "Pwd01", "Pwd01")]
		[TestCase(PasswordRule.PasswordPlusFileNameNumbers, "test123.jpg", "Pwd01", "Pwd01123")]
		[TestCase(PasswordRule.PasswordPlusFileNameNumbers, "test123.mp4", "Pwd01", "Pwd01123")]
		[TestCase(PasswordRule.PasswordPlusFileNameNumbers, "test.jpg", "Pwd01", "Pwd01")]
		[TestCase(PasswordRule.PasswordPlusFileNameNumbers, "test.mp4", "Pwd01", "Pwd01")]
		[TestCase(PasswordRule.FileNameNumbersPlusPassword, "test123.jpg", "Pwd01", "123Pwd01")]
		[TestCase(PasswordRule.FileNameNumbersPlusPassword, "test123.mp4", "Pwd01", "123Pwd01")]
		[TestCase(PasswordRule.FileNameNumbersPlusPassword, "test.jpg", "Pwd01", "Pwd01")]
		[TestCase(PasswordRule.FileNameNumbersPlusPassword, "test.mp4", "Pwd01", "Pwd01")]
		public void PredefinedRules(PasswordRule passwordRule, string fileName, string password, string result)
		{
			var parser = new PasswordGenerator(passwordRule, null, password);
			Assert.AreEqual(result, parser.GetPassword(fileName));
		}
    }
}
