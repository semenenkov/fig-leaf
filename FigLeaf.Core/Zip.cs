using System;
using System.IO;
using System.Text;

using ICSharpCode.SharpZipLib.Zip;

namespace FigLeaf.Core
{
	public class Zip
	{
		private readonly string _masterPassword;

		public Zip(string masterPassword)
		{
			_masterPassword = masterPassword;
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
			var sbPassword = new StringBuilder();

			string nameWithoutExt = Path.GetFileNameWithoutExtension(file.FullName);
			foreach (char c in nameWithoutExt)
				if (Char.IsDigit(c))
					sbPassword.Append(c);

			sbPassword.Append(_masterPassword);
			
			return sbPassword.ToString();
		}
	}
}
