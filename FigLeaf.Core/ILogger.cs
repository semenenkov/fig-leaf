namespace FigLeaf.Core
{
	public interface ILogger
	{
		void Start();
		void Log(bool isDetail, string message);
		void LogProgress();
		void Stop();
		bool IsRunning { get; }
	}
}
