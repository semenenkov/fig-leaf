using FigLeaf.Core;

namespace FigLeaf.Console
{
	internal class Logger: ILogger
	{
		private readonly bool _isDetail;
		private bool _newLineOnLog;

		public Logger(bool isDetail)
		{
			_isDetail = isDetail;
		}

		public void Reset()
		{
			_newLineOnLog = false;
		}

		public void Log(bool isDetail, string message)
		{
			if (isDetail && !_isDetail)
				return;

			if (_newLineOnLog)
			{
				System.Console.WriteLine();
				_newLineOnLog = false;
			}

			System.Console.WriteLine(message);
		}

		public void LogProgress()
		{
			if (_isDetail)
				return;

			System.Console.Write('.');
			_newLineOnLog = true;
		}
	}
}
