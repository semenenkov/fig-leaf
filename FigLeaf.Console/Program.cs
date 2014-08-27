using System;
using System.Linq;
using System.Threading;
using FigLeaf.Core;

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
					if (string.IsNullOrEmpty(cmd.UnpackTarget))
					{
						foreach (DirPair dirPair in settings.Dirs)
						{
							var fileProcessor = new DirPairProcessor(dirPair, settings, logger);
							fileProcessor.Pack(CancellationToken.None);
						}
					}
					else
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
						{
							logger.Log(false, Core.Properties.Resources.Console_RestoreSourceNotDefined);
						}
						else
						{
							var fileProcessor = new DirPairProcessor(dirPair, settings, logger);
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
	}
}
