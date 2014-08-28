using System;
using System.IO;
using System.Text;

using ICSharpCode.SharpZipLib.Zip;

namespace FigLeaf.Core
{
	public class Zip
	{
		private readonly string _masterPassword;
		private readonly PasswordRule _passwordRule;

		public Zip(string masterPassword, PasswordRule passwordRule)
		{
			_masterPassword = masterPassword;
			_passwordRule = passwordRule;
		}

		public void Pack(FileInfo sourceFile, string targetPath)
		{
			var zip = new FastZip();
			zip.Password = GetPassword(sourceFile);
			zip.CreateZip(targetPath, sourceFile.Directory.FullName, true, sourceFile.Name);
		}

		public bool Unpack(string sourcePath, FileInfo targetFile)
		{
			var zip = new FastZip();
			zip.Password = GetPassword(targetFile);
			try
			{
				zip.ExtractZip(sourcePath, targetFile.Directory.FullName, FastZip.Overwrite.Prompt, name => true, "", "", true);
				return true;
			}
			catch
			{
				// most likely it is a thumbnail
				return false;
			}
		}

		private string GetPassword(FileInfo file)
		{
			string fileNameNumbers = _passwordRule == PasswordRule.Password
				? null
				: GetFileNameNumbers(file.FullName);

			switch (_passwordRule)
			{
				case PasswordRule.FileNameNumbersPlusPassword:
					return fileNameNumbers + _masterPassword;
				case PasswordRule.PasswordPlusFileNameNumbers:
					return _masterPassword + fileNameNumbers;
				default:
					return _masterPassword;
			}
		}

		private string GetFileNameNumbers(string fileName)
		{
			var sbResult = new StringBuilder();
			string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
			foreach (char c in nameWithoutExt)
				if (Char.IsDigit(c))
					sbResult.Append(c);
			return sbResult.ToString();
		}
	}
}
