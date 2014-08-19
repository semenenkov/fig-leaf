using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using FigLeaf.Core;

namespace FigLeaf.UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, ILogger
	{
		public ISettings Settings { get; set; }

		private readonly ILogger _logger;
		private CancellationTokenSource _cancellationTokenSource;

		public MainWindow()
		{
			InitializeComponent();

			Settings = new AppConfigSettings();
			DataContext = this;
			_logger = this;
		}

		#region ILogger
		public void Reset()
		{
			txtLog.Text = null;
		}

		public void Log(bool isDetail, string message)
		{
			if (isDetail && !Settings.DetailedLogging)
				return;

			Dispatcher.Invoke(new Action(() =>
			{
				string text = txtLog.Text;
				if (!string.IsNullOrEmpty(text))
					text = text + Environment.NewLine;

				txtLog.Text = text + message;
				txtLog.CaretIndex = txtLog.Text.Length;
				txtLog.ScrollToEnd();
			}));
		}
		#endregion

		private void WindowClosing(object sender, CancelEventArgs e)
		{
			Settings.Save();
		}

		private void UpdateTarget(object sender, RoutedEventArgs e)
		{
			LayoutRoot.IsEnabled = false;
			_cancellationTokenSource = new CancellationTokenSource();
			_logger.Reset();
			var fileProcessor = new BatchFileProcessor(Settings, _logger);
			Task.Factory
				.StartNew(() => fileProcessor.Pack(_cancellationTokenSource.Token), _cancellationTokenSource.Token)
				.ContinueWith(o => Dispatcher.Invoke(new Action(() => { LayoutRoot.IsEnabled = true; })));
		}

		private void RestoreTarget(object sender, RoutedEventArgs e)
		{
			string targetPath;

			using (var dlg = new FolderBrowserDialog())
			{
				dlg.Description = "Specify empty folder to unpack files";
				dlg.SelectedPath = Settings.SourceDir;
				dlg.ShowNewFolderButton = true;

				DialogResult result = dlg.ShowDialog();
				if (result != System.Windows.Forms.DialogResult.OK)
					return;

				targetPath = dlg.SelectedPath;
			}

			LayoutRoot.IsEnabled = false;
			_cancellationTokenSource = new CancellationTokenSource();
			_logger.Reset();
			var fileProcessor = new BatchFileProcessor(Settings, _logger);
			Task.Factory
				.StartNew(() => fileProcessor.Unpack(targetPath, _cancellationTokenSource.Token), _cancellationTokenSource.Token)
				.ContinueWith(o => Dispatcher.Invoke(new Action(() => { LayoutRoot.IsEnabled = true; })));
		}

		private void CancelAction(object sender, RoutedEventArgs e)
		{
			if (_cancellationTokenSource == null)
				return;
			
			_cancellationTokenSource.Cancel();
		}
	}
}
