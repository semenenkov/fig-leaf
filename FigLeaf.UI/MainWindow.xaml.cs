using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WinForms = System.Windows.Forms;

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
			Settings = new AppConfigSettings();
			Core.Properties.Resources.Culture = CultureInfo.GetCultureInfo(Settings.Culture);

			InitializeComponent();

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

			using (var dlg = new WinForms.FolderBrowserDialog())
			{
				dlg.Description = "Specify empty folder to unpack files";
				dlg.SelectedPath = Settings.SourceDir;
				dlg.ShowNewFolderButton = true;

				WinForms.DialogResult result = dlg.ShowDialog();
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

		private void SwitchLanguage(object sender, RoutedEventArgs e)
		{
			var button = (ToggleButton) sender;
			button.IsChecked = ChangeCulture(button.Tag.ToString());
		}

		private bool ChangeCulture(string cultureCode)
		{
			if (cultureCode == Core.Properties.Resources.Culture.Name)
				return true;

			try
			{
				var question = new StringBuilder();
				question.Append(Core.Properties.Resources.Ui_Dialogs_SwitchLanguage);
				question.Append(Environment.NewLine);
				Core.Properties.Resources.Culture = CultureInfo.GetCultureInfo(cultureCode);
				question.Append(Core.Properties.Resources.Ui_Dialogs_SwitchLanguage);
				
				if (MessageBox.Show(question.ToString(), null, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					Settings.Culture = cultureCode;
					Settings.Save();
					Process.Start(System.Windows.Forms.Application.ExecutablePath);
					Application.Current.Shutdown();
				}
				else
				{
					Core.Properties.Resources.Culture = CultureInfo.GetCultureInfo(Settings.Culture);
					return false;
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				Core.Properties.Resources.Culture = CultureInfo.GetCultureInfo(Settings.Culture);
			}

			return true;
		}
	}
}
