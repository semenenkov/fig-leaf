using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FigLeaf.Core
{
	public class BatchFileProcessor
	{
		private readonly string _sourceDirPath;
		private readonly string _targetDirPath;
		private readonly Zip _zip;
		private readonly Thumbnail _thumbnail;

		public int CreatedDirCount { get; private set; }
		public int CreatedFileCount { get; private set; }
		public int CreatedThumbnailCount { get; private set; }
		public int RemovedObsoleteFileCount { get; private set; }
		public int RemovedObsoleteThumbnailCount { get; private set; }
		public int RemovedTargetWithoutSourceFileCount { get; private set; }
		public int RemovedTargetWithoutSourceDirCount { get; private set; }

		public BatchFileProcessor(ISettings settings)
		{
			_sourceDirPath = settings.SourceDir;
			_targetDirPath = settings.TargetDir;
			_zip = new Zip(settings.MasterPassword);
			_thumbnail = new Thumbnail(settings, Console.WriteLine);
		}

		public void Pack()
		{
			var sourceDir = new DirectoryInfo(_sourceDirPath);
			var targetDir = new DirectoryInfo(_targetDirPath);

			if (!sourceDir.Exists)
				throw new ApplicationException("Source folder does not exist");

			if (!targetDir.Exists) targetDir.Create();

			ProcessDir(sourceDir, targetDir, true, PackFile);
		}

		public void Unpack(string targetPath)
		{
			var sourceDir = new DirectoryInfo(_targetDirPath);
			var targetDir = new DirectoryInfo(targetPath);

			if (!sourceDir.Exists)
				throw new ApplicationException("Source folder does not exist");

			if (!targetDir.Exists)
			{
				targetDir.Create();
			}
			else if (targetDir.GetFiles().Length > 0 || targetDir.GetDirectories().Length > 0)
			{
				throw new ApplicationException("Target dir must be empty");
			}

			ProcessDir(sourceDir, targetDir, false, UnpackFile);
		}

		private void ProcessDir(
			DirectoryInfo sourceDir, 
			DirectoryInfo targetDir, 
			bool cleanTarget,
			Action<FileInfo, DirectoryInfo, HashSet<string>> processFile)
		{
			// 1. process own files
			IEnumerable<FileInfo> sourceFiles = sourceDir.GetFiles();
			var targetValidFileNames = new HashSet<string>();
			foreach (var file in sourceFiles)
			{
				processFile(file, targetDir, targetValidFileNames);
			}

			// 2. remove unexisting target files
			if (cleanTarget)
			{
				foreach (var targetFile in targetDir.GetFiles())
				{
					if (!targetValidFileNames.Contains(targetFile.Name))
					{
						targetFile.Delete();
						RemovedTargetWithoutSourceFileCount++;
					}
				}
			}

			// 3. process subdirs
			IEnumerable<DirectoryInfo> subDirs = sourceDir.GetDirectories();
			foreach (var sourceSubDir in subDirs)
			{
				var targetSubDir = new DirectoryInfo(Path.Combine(targetDir.FullName, sourceSubDir.Name));
				if (!targetSubDir.Exists)
				{
					targetSubDir.Create();
					CreatedDirCount++;
				}
				
				ProcessDir(sourceSubDir, targetSubDir, cleanTarget, processFile);
			}

			// 4. remove unexisting target dirs
			if (cleanTarget)
			{
				foreach (var targetSubDir in targetDir.GetDirectories())
				{
					if (subDirs.All(d => d.Name != targetSubDir.Name))
					{
						int filesToRemove = targetSubDir.GetFiles(".", SearchOption.AllDirectories).Length;
						int dirsToRemove = targetSubDir.GetDirectories("*.*", SearchOption.AllDirectories).Length;
						targetSubDir.Delete(true);
						RemovedTargetWithoutSourceFileCount = RemovedTargetWithoutSourceFileCount + filesToRemove;
						RemovedTargetWithoutSourceDirCount = RemovedTargetWithoutSourceDirCount + 1 + dirsToRemove;
					}
				}
			}
		}

		private void PackFile(FileInfo sourceFile, DirectoryInfo targetDir, HashSet<string> targetValidFileNames)
		{
			string targetFilePath = Path.Combine(targetDir.FullName, sourceFile.Name);
			DateTime sourceFileTime = File.GetLastWriteTime(sourceFile.FullName);

			// 1. If the same name exists 
			// - with different time - remove
			// - with same time - skip file creation
			
			bool skipExisting = false;
			if (File.Exists(targetFilePath))
			{
				DateTime targetFileTime = File.GetLastWriteTime(targetFilePath);
				if (targetFileTime.Equals(sourceFileTime))
				{
					skipExisting = true;
				}
				else
				{
					File.Delete(targetFilePath);
					RemovedObsoleteFileCount++;
				}
			}

			if (!skipExisting)
			{
				_zip.Pack(sourceFile, targetFilePath);
				File.SetLastWriteTime(targetFilePath, sourceFileTime);
				CreatedFileCount++;
			}

			targetValidFileNames.Add(Path.GetFileName(targetFilePath));

			// 2. target thumbnail exists
			// - with different time - remove
			// - with same time - skip file creation

			bool isVideo;
			skipExisting = false;
			string thumbnailFileName = _thumbnail.GetThumbnailFileName(sourceFile.FullName, out isVideo);
			string thumbnailFilePath = Path.Combine(targetDir.FullName, thumbnailFileName);
			if (File.Exists(thumbnailFilePath))
			{
				DateTime targetFileTime = File.GetLastWriteTime(thumbnailFilePath);
				if (targetFileTime.Equals(sourceFileTime))
				{
					skipExisting = true;
				}
				else
				{
					File.Delete(thumbnailFilePath);
					RemovedObsoleteThumbnailCount++;
				}
			}

			if (!skipExisting)
			{
				if (isVideo)
					_thumbnail.MakeForVideo(sourceFile.FullName, thumbnailFilePath);
				else
					_thumbnail.MakeForPhoto(sourceFile.FullName, thumbnailFilePath);
				File.SetLastWriteTime(thumbnailFilePath, sourceFileTime);
				CreatedThumbnailCount++;
			}

			targetValidFileNames.Add(Path.GetFileName(thumbnailFilePath));
		}

		private void UnpackFile(FileInfo sourceFile, DirectoryInfo targetDir, HashSet<string> targetValidFileNames)
		{
			string targetFilePath = Path.Combine(targetDir.FullName, sourceFile.Name);
			DateTime sourceFileTime = File.GetLastWriteTime(sourceFile.FullName);

			if (!_zip.Unpack(sourceFile.FullName, new FileInfo(targetFilePath))) 
				return;

			File.SetLastWriteTime(targetFilePath, sourceFileTime);
			CreatedFileCount++;
		}
	}
}
