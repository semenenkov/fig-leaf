using System;
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
				fileProcessor.Pack();
				//fileProcessor.Unpack(@"d:\Patrick\FigLeaf\trunk\!_test\source2");
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
