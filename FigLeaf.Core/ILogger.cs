namespace FigLeaf.Core
{
	public interface ILogger
	{
		void Reset();
		void Log(bool isDetail, string message);
		void LogProgress();
	}
}
