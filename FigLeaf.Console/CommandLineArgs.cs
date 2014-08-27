using System;
using System.Linq;

namespace FigLeaf.Console
{
	public class CommandLineArgs
	{
		private readonly string[] _prmDetailedLogging = new[] { "l", "log" };
		private readonly string[] _prmUnpackTarget = new[] { "r", "restore" };
		private readonly string[] _prmUnpackSource = new[] { "f", "from" };

		public bool DetailedLogging { get; private set; }
		public string UnpackTarget { get; private set; }
		public string UnpackSource { get; private set; }

		public CommandLineArgs(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				string arg = args[i];
				if (arg == null || arg.Length < 2)
					continue;

				if (!(arg.StartsWith("-") || arg.StartsWith("/")))
					continue;

				arg = arg.Substring(1);
				if (_prmDetailedLogging.Any(a => string.Equals(a, arg, StringComparison.OrdinalIgnoreCase)))
					DetailedLogging = true;
				else if (i == args.Length - 1) // all other options require next arg
					break;

				if (_prmUnpackTarget.Any(a => string.Equals(a, arg, StringComparison.OrdinalIgnoreCase)))
					UnpackTarget = args[i + 1];

				if (_prmUnpackSource.Any(a => string.Equals(a, arg, StringComparison.OrdinalIgnoreCase)))
					UnpackSource = args[i + 1];
			}
		}
	}
}
