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
		private readonly ILogger _logger;

		private int _processedDirCount;
		private int _processedFileCount;
		private int _createdDirCount;
		private int _createdFileCount;
		private int _createdThumbnailCount;
		private int _removedObsoleteFileCount;
		private int _removedObsoleteThumbnailCount;
		private int _removedTargetWithoutSourceFileCount;
		private int _removedTargetWithoutSourceDirCount;

		public BatchFileProcessor(ISettings settings, ILogger logger)
		{
			_sourceDirPath = settings.SourceDir;
			_targetDirPath = settings.TargetDir;
			_zip = new Zip(settings.MasterPassword);
			_thumbnail = new Thumbnail(settings, Console.WriteLine);
			_logger = logger;
		}

		public void Pack()
		{
			try
			{
				var sourceDir = new DirectoryInfo(_sourceDirPath);
				var targetDir = new DirectoryInfo(_targetDirPath);

				if (!sourceDir.Exists)
					throw new ApplicationException("Source folder does not exist");

				if (!targetDir.Exists) targetDir.Create();

				ProcessDir(sourceDir, targetDir, true, PackFile);

				_logger.Log(false, string.Format("Processed source folders: {0}", _processedDirCount));
				_logger.Log(false, string.Format("Processed source files: {0}", _processedFileCount));
				_logger.Log(false, string.Format("Created target folders: {0}", _createdDirCount));
				_logger.Log(false, string.Format("Created target files: {0}", _createdFileCount));
				_logger.Log(false, string.Format("Created target thumbnails: {0}", _createdThumbnailCount));
				_logger.Log(false, string.Format("Removed obsolete target files: {0}", _removedObsoleteFileCount));
				_logger.Log(false, string.Format("Removed obsolete target thumbnails: {0}", _removedObsoleteThumbnailCount));
				_logger.Log(false, string.Format("Removed target files without source: {0}", _removedTargetWithoutSourceFileCount));
				_logger.Log(false, string.Format("Removed target folders without source: {0}", _removedTargetWithoutSourceDirCount));
			}
			catch (Exception e)
			{
				_logger.Log(false, "Error: " + e.Message);
			}
		}

		public void Unpack(string targetPath)
		{
			try
			{
				var sourceDir = new DirectoryInfo(_targetDirPath);
				var targetDir = new DirectoryInfo(targetPath);

				if (!sourceDir.Exists)
					throw new ApplicationException("Source folder does not exist");

				if (!targetDir.Exists)
					targetDir.Create();
				else if (targetDir.GetFiles().Length > 0 || targetDir.GetDirectories().Length > 0)
					throw new ApplicationException("Target dir must be empty");

				ProcessDir(sourceDir, targetDir, false, UnpackFile);
			}
			catch (Exception e)
			{
				_logger.Log(false, "Error: " + e.Message);
			}
		}

		private void ProcessDir(
			DirectoryInfo sourceDir, 
			DirectoryInfo targetDir, 
			bool cleanTarget,
			Action<FileInfo, DirectoryInfo, HashSet<string>> processFile)
		{
			_processedDirCount++;

			// 1. process own files
			IEnumerable<FileInfo> sourceFiles = sourceDir.GetFiles();
			var targetValidFileNames = new HashSet<string>();
			foreach (var file in sourceFiles)
			{
				_processedFileCount++;
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
						_removedTargetWithoutSourceFileCount++;
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
					_createdDirCount++;
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
						_removedTargetWithoutSourceFileCount = _removedTargetWithoutSourceFileCount + filesToRemove;
						_removedTargetWithoutSourceDirCount = _removedTargetWithoutSourceDirCount + 1 + dirsToRemove;
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
					_removedObsoleteFileCount++;
				}
			}

			if (!skipExisting)
			{
				_zip.Pack(sourceFile, targetFilePath);
				File.SetLastWriteTime(targetFilePath, sourceFileTime);
				_createdFileCount++;
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
					_removedObsoleteThumbnailCount++;
				}
			}

			if (!skipExisting)
			{
				if (isVideo)
					_thumbnail.MakeForVideo(sourceFile.FullName, thumbnailFilePath);
				else
					_thumbnail.MakeForPhoto(sourceFile.FullName, thumbnailFilePath);
				File.SetLastWriteTime(thumbnailFilePath, sourceFileTime);
				_createdThumbnailCount++;
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
			_createdFileCount++;
		}
	}
}
