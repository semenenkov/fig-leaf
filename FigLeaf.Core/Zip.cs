using System.IO;

using ICSharpCode.SharpZipLib.Zip;

namespace FigLeaf.Core
{
	public class Zip
	{
		private readonly PasswordRules.PasswordGenerator _passwordGenerator;

		public Zip(PasswordRules.PasswordGenerator passwordGenerator)
		{
			_passwordGenerator = passwordGenerator;
		}

		public void Pack(FileInfo sourceFile, string targetPath)
		{
			var zip = new FastZip();
			zip.Password = _passwordGenerator.GetPassword(sourceFile.Name);
			zip.CreateZip(targetPath, sourceFile.Directory.FullName, true, sourceFile.Name);
		}

		public bool Unpack(string sourcePath, FileInfo targetFile)
		{
			var zip = new FastZip();
			zip.Password = _passwordGenerator.GetPassword(targetFile.Name);
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
	}
}
