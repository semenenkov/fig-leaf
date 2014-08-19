using System;
using System.Linq;

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
				var fileProcessor = new BatchFileProcessor(new AppConfigSettings(), logger);

				if (argUnpackFolder == null)
					fileProcessor.Pack();
				else
					fileProcessor.Unpack(argUnpackFolder);
			}
			catch (Exception e)
			{
				logger.Log(false, "Error: " + e.Message);
			}

#if DEBUG
			System.Console.Read();
#endif
		}
	}
}
