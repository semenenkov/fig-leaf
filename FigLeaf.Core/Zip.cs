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
			try
			{
				using (FileStream fs = File.OpenRead(sourcePath))
				{
					var zipFile = new ZipFile(fs);
					zipFile.Password = _passwordGenerator.GetPassword(targetFile.Name);

					foreach (ZipEntry zipEntry in zipFile)
					{
						if (!zipEntry.IsFile)
							continue;

						using (Stream zipStream = zipFile.GetInputStream(zipEntry))
						using (FileStream extractStream = File.Create(targetFile.FullName))
							CopyStream(zipStream, extractStream);

						// expect to have one file per zip, target file is unique
						break;
					}
				}
				return true;
			}
			catch
			{
				// most likely it is a thumbnail, not a zip
				return false;
			}
		}

		public static void CopyStream(Stream sourceStream, Stream targetStream)
		{
			var buffer = new byte[0x10000];
			int n;
			while ((n = sourceStream.Read(buffer, 0, buffer.Length)) != 0)
				targetStream.Write(buffer, 0, n);
		}
	}
}
