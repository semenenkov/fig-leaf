﻿using System;
using FigLeaf.Core;

namespace FigLeaf.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var logger = new Logger(false);
			try
			{
				var fileProcessor = new BatchFileProcessor(new AppConfigSettings(), logger);

				if (args == null || args.Length == 0)
					fileProcessor.Pack();
				else
					fileProcessor.Unpack(args[0]);
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
