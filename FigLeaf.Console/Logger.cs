using FigLeaf.Core;

namespace FigLeaf.Console
{
	internal class Logger: ILogger
	{
		private readonly bool _isDetail;

		public Logger(bool isDetail)
		{
			_isDetail = isDetail;
		}

		public void Reset()
		{
		}

		public void Log(bool isDetail, string message)
		{
			if (isDetail && !_isDetail)
				return;

			System.Console.WriteLine(message);
		}
	}
}
