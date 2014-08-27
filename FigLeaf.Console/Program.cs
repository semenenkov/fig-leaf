using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FigLeaf.Core;

namespace FigLeaf.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			string[] detailedLoggingArgs = new[] {"-l", "/l", "-log", "/log"};

			bool argDetailedLogging = false;
			string argUnpackFolder = null;

			foreach (var arg in args)
			{
				if (detailedLoggingArgs.Any(a => string.Equals(a, arg, StringComparison.OrdinalIgnoreCase)))
					argDetailedLogging = true;
				else
					argUnpackFolder = arg;
			}

			var logger = new Logger(argDetailedLogging);
			try
			{
				var settings = Settings.ReadFromFile(false);
				if (settings == null)
				{
					logger.Log(false, string.Format(Core.Properties.Resources.Common_ErrorFormat, Core.Properties.Resources.Console_NoSettings));
				}
				else
				{
					foreach (DirPair dirPair in settings.Dirs)
					{
						var fileProcessor = new DirPairProcessor(dirPair, settings, logger);
						if (argUnpackFolder == null)
							fileProcessor.Pack(CancellationToken.None);
						else
							fileProcessor.Unpack(argUnpackFolder, CancellationToken.None);
					}
				}
			}
			catch (Exception e)
			{
				logger.Log(false, string.Format(Core.Properties.Resources.Common_ErrorFormat, e.Message));
			}

#if DEBUG
			System.Console.Read();
#endif
		}
	}
}
