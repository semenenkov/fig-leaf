using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

			if (!string.IsNullOrEmpty(txtLog.Text))
				txtLog.Text = txtLog.Text + Environment.NewLine;

			txtLog.Text = txtLog.Text + message;

			txtLog.CaretIndex = txtLog.Text.Length;
			txtLog.ScrollToEnd();
		}
		#endregion

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Settings.Save();
		}

		private void UpdateTarget(object sender, RoutedEventArgs e)
		{
			_logger.Reset();
			var fileProcessor = new BatchFileProcessor(Settings, _logger);
			fileProcessor.Pack();
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

			_logger.Reset();
			var fileProcessor = new BatchFileProcessor(Settings, _logger);
			fileProcessor.Unpack(targetPath);
		}
	}
}
