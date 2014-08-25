using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

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
		private string _excludeFolder;

		public BatchFileProcessor(ISettings settings, ILogger logger)
		{
			_sourceDirPath = settings.SourceDir;
			_targetDirPath = settings.TargetDir;
			_zip = new Zip(settings.MasterPassword);
			_thumbnail = new Thumbnail(settings, Console.WriteLine);
			_logger = logger;

			if (settings.ExcludeFigLeafDir)
			{
				string exePath = NormalizePath(Path.GetDirectoryName(Application.ExecutablePath));
				string normalizedSourcePath = NormalizePath(_sourceDirPath);
				if (exePath.StartsWith(normalizedSourcePath)) // is subfolder
				{
					_excludeFolder = exePath;
				}
			}

			_logger.Log(false, Properties.Resources.Core_FileProcessor_Start);
		}

		public void Pack(CancellationToken cancellationToken)
		{
			try
			{
				var sourceDir = new DirectoryInfo(_sourceDirPath);
				var targetDir = new DirectoryInfo(_targetDirPath);

				if (!sourceDir.Exists)
					throw new ApplicationException(Properties.Resources.Core_FileProcessor_NoSourceDirError);

				if (!targetDir.Exists)
				{
					_logger.Log(true, string.Format(Properties.Resources.Core_FileProcessor_CreatedDirFormat, _targetDirPath));
					targetDir.Create();
					_createdDirCount++;
				}

				ProcessDir(sourceDir, targetDir, true, PackFile, cancellationToken);
				LogSummary();
			}
			catch (Exception e)
			{
				_logger.Log(false, string.Format(Properties.Resources.Core_FileProcessor_ErrorFormat, e.Message));
			}
		}

		public void Unpack(string targetPath, CancellationToken cancellationToken)
		{
			try
			{
				var sourceDir = new DirectoryInfo(_targetDirPath);
				var targetDir = new DirectoryInfo(targetPath);

				if (!sourceDir.Exists)
					throw new ApplicationException(Properties.Resources.Core_FileProcessor_NoSourceDirError);

				if (!targetDir.Exists)
					targetDir.Create();
				else if (targetDir.GetFiles().Length > 0 || targetDir.GetDirectories().Length > 0)
					throw new ApplicationException(Properties.Resources.Core_FileProcessor_NonEmptyTargetDirError);

				ProcessDir(sourceDir, targetDir, false, UnpackFile, cancellationToken);
				LogSummary();
			}
			catch (Exception e)
			{
				_logger.Log(false, string.Format(Properties.Resources.Core_FileProcessor_ErrorFormat, e.Message));
			}
		}

		private void LogSummary()
		{
			_logger.Log(false, string.Format(Properties.Resources.Core_FileProcessor_LogSum_ProcessedSourceDirsFormat, _processedDirCount));
			_logger.Log(false, string.Format(Properties.Resources.Core_FileProcessor_LogSum_ProcessedSourceFilesFormat, _processedFileCount));
			_logger.Log(false, string.Format(Properties.Resources.Core_FileProcessor_LogSum_CreatedTargetDirsFormat, _createdDirCount));
			_logger.Log(false, string.Format(Properties.Resources.Core_FileProcessor_LogSum_CreatedTargetFilesFormat, _createdFileCount));
			_logger.Log(false, string.Format(Properties.Resources.Core_FileProcessor_LogSum_CreatedTargetThumbsFormat, _createdThumbnailCount));
			_logger.Log(false, string.Format(Properties.Resources.Core_FileProcessor_LogSum_RemovedObsoleteTargetFilesFormat, _removedObsoleteFileCount));
			_logger.Log(false, string.Format(Properties.Resources.Core_FileProcessor_LogSum_RemovedObsoleteTargetThumbsFormat, _removedObsoleteThumbnailCount));
			_logger.Log(false, string.Format(Properties.Resources.Core_FileProcessor_LogSum_RemovedTargetFilesWoSourceFormat, _removedTargetWithoutSourceFileCount));
			_logger.Log(false, string.Format(Properties.Resources.Core_FileProcessor_LogSum_RemovedTargetDirsWoSourceFormat, _removedTargetWithoutSourceDirCount));
		}

		private void ProcessDir(
			DirectoryInfo sourceDir, 
			DirectoryInfo targetDir, 
			bool cleanTarget,
			Action<FileInfo, DirectoryInfo, HashSet<string>, CancellationToken> processFile,
			CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
				return;

			if (!string.IsNullOrEmpty(_excludeFolder))
			{
				if (_excludeFolder == NormalizePath(sourceDir.FullName))
				{
					_logger.Log(true, string.Format(Properties.Resources.Core_FileProcessor_SkipFigLeafDirFormat, _excludeFolder));
					// clear this path to skip this check for other folders
					_excludeFolder = null;
					return;
				}
			}

			_processedDirCount++;

			// 1. process own files
			IEnumerable<FileInfo> sourceFiles = sourceDir.GetFiles();
			var targetValidFileNames = new HashSet<string>();
			foreach (var file in sourceFiles)
			{
				_processedFileCount++;
				processFile(file, targetDir, targetValidFileNames, cancellationToken);
			}

			// 2. remove unexisting target files
			if (cleanTarget)
			{
				foreach (var targetFile in targetDir.GetFiles())
				{
					if (!targetValidFileNames.Contains(targetFile.Name))
					{
						_logger.Log(true, string.Format(Properties.Resources.Core_FileProcessor_DeletingTargetFileWoSourceFormat, targetFile.Name));
						targetFile.Delete();
						_removedTargetWithoutSourceFileCount++;
					}
				}
			}

			if (cancellationToken.IsCancellationRequested)
				return;

			// 3. process subdirs
			IEnumerable<DirectoryInfo> subDirs = sourceDir.GetDirectories();
			foreach (var sourceSubDir in subDirs)
			{
				var targetSubDir = new DirectoryInfo(Path.Combine(targetDir.FullName, sourceSubDir.Name));
				if (!targetSubDir.Exists)
				{
					_logger.Log(true, string.Format(Properties.Resources.Core_FileProcessor_CreatingTargetDirFormat, targetSubDir.FullName));
					targetSubDir.Create();
					_createdDirCount++;
				}

				ProcessDir(sourceSubDir, targetSubDir, cleanTarget, processFile, cancellationToken);
			}

			if (cancellationToken.IsCancellationRequested)
				return;

			// 4. remove unexisting target dirs
			if (cleanTarget)
			{
				foreach (var targetSubDir in targetDir.GetDirectories())
				{
					if (subDirs.All(d => d.Name != targetSubDir.Name))
					{
						int filesToRemove = targetSubDir.GetFiles(".", SearchOption.AllDirectories).Length;
						int dirsToRemove = targetSubDir.GetDirectories("*.*", SearchOption.AllDirectories).Length;
						_logger.Log(true, string.Format(
							Properties.Resources.Core_FileProcessor_DeletingTargetDirWoSourceFormat, 
							targetSubDir, dirsToRemove, filesToRemove));
						targetSubDir.Delete(true);
						_removedTargetWithoutSourceFileCount = _removedTargetWithoutSourceFileCount + filesToRemove;
						_removedTargetWithoutSourceDirCount = _removedTargetWithoutSourceDirCount + 1 + dirsToRemove;
					}
				}
			}
		}

		private void PackFile(
			FileInfo sourceFile, 
			DirectoryInfo targetDir, 
			HashSet<string> targetValidFileNames,
			CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
				return;

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
					_logger.Log(true, string.Format(Properties.Resources.Core_FileProcessor_DeletingObsoleteTargetFileFormat, targetFilePath));
					File.Delete(targetFilePath);
					_removedObsoleteFileCount++;
				}
			}

			if (!skipExisting)
			{
				_logger.Log(true, string.Format(Properties.Resources.Core_FileProcessor_PackingFileFormat, sourceFile.FullName, targetFilePath));
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
					_logger.Log(true, string.Format(Properties.Resources.Core_FileProcessor_DeletingObsoleteThumbFormat, thumbnailFilePath));
					File.Delete(thumbnailFilePath);
					_removedObsoleteThumbnailCount++;
				}
			}

			if (!skipExisting)
			{
				_logger.Log(true, string.Format(Properties.Resources.Core_FileProcessor_CreatingThumbFormat, thumbnailFilePath));
				if (isVideo)
					_thumbnail.MakeForVideo(sourceFile.FullName, thumbnailFilePath);
				else
					_thumbnail.MakeForPhoto(sourceFile.FullName, thumbnailFilePath);
				File.SetLastWriteTime(thumbnailFilePath, sourceFileTime);
				_createdThumbnailCount++;
			}

			targetValidFileNames.Add(Path.GetFileName(thumbnailFilePath));
		}

		private void UnpackFile(
			FileInfo sourceFile, 
			DirectoryInfo targetDir, 
			HashSet<string> targetValidFileNames,
			CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
				return;

			string targetFilePath = Path.Combine(targetDir.FullName, sourceFile.Name);
			DateTime sourceFileTime = File.GetLastWriteTime(sourceFile.FullName);

			if (!_zip.Unpack(sourceFile.FullName, new FileInfo(targetFilePath))) 
				return;

			_logger.Log(true, string.Format(Properties.Resources.Core_FileProcessor_UnpackingFileFormat, sourceFile.FullName, targetFilePath));
			File.SetLastWriteTime(targetFilePath, sourceFileTime);
			_createdFileCount++;
		}

		private static string NormalizePath(string path)
		{
			return Path.GetFullPath(new Uri(path).LocalPath)
				.TrimEnd(Path.DirectorySeparatorChar, Path.DirectorySeparatorChar)
				.ToUpperInvariant();
		}
	}
}
