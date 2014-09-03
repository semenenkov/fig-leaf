using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FigLeaf.Core;
using FigLeaf.Core.PasswordRules;

namespace FigLeaf.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var cmd = new CommandLineArgs(args);
			var logger = new Logger(cmd.DetailedLogging);

			try
			{
				var settings = Settings.ReadFromFile(false);
				Utils.SetupCulture(settings);
				if (settings == null)
				{
					logger.Log(false, string.Format(Core.Properties.Resources.Common_ErrorFormat, Core.Properties.Resources.Console_NoSettings));
				}
				else
				{
					var passwordRuleParser = new PasswordGenerator(settings.PasswordRule, settings.CustomPasswordRule, settings.MasterPassword);
					var zip = new Zip(passwordRuleParser);

					if (string.IsNullOrEmpty(cmd.UnpackTarget))
					{
						IEnumerable<DirPair> dirPairs = settings.HasMultipleDirs
							? settings.Dirs
							: settings.Dirs.Take(1);
						var thumbnail = settings.EnableThumbnails 
							? new Thumbnail(settings)
							: null;

						foreach (DirPair dirPair in dirPairs)
						{
							try
							{
								var fileProcessor = new DirPairProcessor(
									dirPair, settings.ArchiveNameRule, settings.ExcludeFigLeafDir, zip, thumbnail, logger);
								fileProcessor.Pack(CancellationToken.None, GetCleanTargetConfirm(settings.ConfirmDelete));
							}
							catch (Exception e)
							{
								logger.Log(false, string.Format(Core.Properties.Resources.Common_ErrorFormat, e.Message));
							}
						}
					}
					else
					{
						var dirPair = GetUnpackDirPair(cmd, settings, logger);
						if (dirPair != null)
						{
							var fileProcessor = new DirPairProcessor(dirPair, settings.ArchiveNameRule, false, zip, null, logger);
							fileProcessor.Unpack(cmd.UnpackTarget, CancellationToken.None);
						}
					}
				}
			}
			catch (Exception e)
			{
				logger.Log(false, string.Format(Core.Properties.Resources.Common_ErrorFormat, e.Message));
			}

#if DEBUG
			System.Console.WriteLine("Press any key to close..");
			System.Console.Read();
#endif
		}

		private static DirPair GetUnpackDirPair(CommandLineArgs cmd, Settings settings, ILogger logger)
		{
			DirPair dirPair = null;

			if (!string.IsNullOrEmpty(cmd.UnpackSource))
			{
				dirPair = settings.Dirs.FirstOrDefault(d =>
				    string.Equals(d.Target, cmd.UnpackSource, StringComparison.InvariantCultureIgnoreCase));
			}
			else if (settings.Dirs.Count == 1 || !settings.HasMultipleDirs)
			{
				dirPair = settings.Dirs[0];
			}

			if (dirPair == null)
				logger.Log(false, Core.Properties.Resources.Console_RestoreSourceNotDefined);

			return dirPair;
		}

		private static Func<string, bool> GetCleanTargetConfirm(bool confirmDelete)
		{
			if (!confirmDelete)
				return (s) => true;

			return GetCleanTargetCosoleConfirm;
		}

		private static bool GetCleanTargetCosoleConfirm(string path)
		{
			System.Console.Write(Core.Properties.Resources.Console_ConfirmDeleteFormat, path);
			string result = System.Console.ReadLine();
			return string.Equals(result, "y", StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
