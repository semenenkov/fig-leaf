using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WinForms = System.Windows.Forms;

using FigLeaf.Core;
using CoreResources = FigLeaf.Core.Properties.Resources;

namespace FigLeaf.UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, ILogger
	{
		public Settings Settings { get; set; }

		private readonly ILogger _logger;
		private CancellationTokenSource _cancellationTokenSource;

		public MainWindow()
		{
			Settings = Settings.ReadFromFile(true);
			Utils.SetupCulture(Settings);

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


		public void LogProgress()
		{
			if (Settings.DetailedLogging)
				return;

			Dispatcher.Invoke(new Action(() =>
			{
				txtLog.Text = txtLog.Text + '.';
				txtLog.CaretIndex = txtLog.Text.Length;
				txtLog.ScrollToEnd();
			}));
		}
		#endregion

		private void WindowClosing(object sender, CancelEventArgs e)
		{
			Settings.SaveToFile();
		}

		private void AddDir(object sender, RoutedEventArgs e)
		{
			Settings.Dirs.Add(new DirPair(null, null));
			grDirs.ItemsSource = null;
			grDirs.ItemsSource = Settings.Dirs;
		}

		private void DelDir(object sender, RoutedEventArgs e)
		{
			if (Settings.Dirs.Count <= 1) return;

			var dirPair = ((Button) sender).DataContext as DirPair;
			if (dirPair == null)
				return;

			Settings.Dirs.Remove(dirPair);
			grDirs.ItemsSource = null;
			grDirs.ItemsSource = Settings.Dirs;
		}

		private void UpdateTarget(object sender, RoutedEventArgs e)
		{
			LayoutRoot.IsEnabled = false;
			_cancellationTokenSource = new CancellationTokenSource();
			_logger.Reset();
			Task.Factory
				.StartNew(() =>
					{
						IEnumerable<DirPair> dirPairs = Settings.HasMultipleDirs
							? Settings.Dirs
							: Settings.Dirs.Take(1);
						foreach (DirPair dirPair in dirPairs)
						{
							var fileProcessor = new DirPairProcessor(dirPair, Settings, _logger);
							fileProcessor.Pack(_cancellationTokenSource.Token, GetCleanTargetConfirm(Settings.ConfirmDelete));
							if (_cancellationTokenSource.IsCancellationRequested)
								break;
						}
					}, 
					_cancellationTokenSource.Token
				)
				.ContinueWith(LogErrorIfAny, TaskContinuationOptions.OnlyOnFaulted)
				.ContinueWith(o => Dispatcher.Invoke(new Action(() => { LayoutRoot.IsEnabled = true; })));
		}

		private static Func<string, bool> GetCleanTargetConfirm(bool confirmDelete)
		{
			if (!confirmDelete)
				return (s) => true;

			return GetCleanTargetUiConfirm;
		}

		private static bool GetCleanTargetUiConfirm(string path)
		{
			return MessageBox.Show(
				string.Format(Core.Properties.Resources.Ui_ConfirmDeleteFormat, path),
				string.Empty, MessageBoxButton.YesNo) == MessageBoxResult.Yes;
		}

		private void RestoreTarget(object sender, RoutedEventArgs e)
		{
			if (Settings.Dirs.Count < 1) 
				return;

			DirPair dirPair = null;
			if (grDirs.Visibility == Visibility.Visible)
			{
				dirPair = ((Button) sender).DataContext as DirPair;
				if (dirPair == null) return;
			}
			else
			{
				dirPair = Settings.Dirs[0];
			}

			string targetPath;
			using (var dlg = new WinForms.FolderBrowserDialog())
			{
				dlg.Description = CoreResources.Ui_Dialogs_RestoreTarget;
				dlg.SelectedPath = dirPair.Source;
				dlg.ShowNewFolderButton = true;

				WinForms.DialogResult result = dlg.ShowDialog();
				if (result != System.Windows.Forms.DialogResult.OK)
					return;

				targetPath = dlg.SelectedPath;
			}

			LayoutRoot.IsEnabled = false;
			_cancellationTokenSource = new CancellationTokenSource();
			_logger.Reset();
			Task.Factory
				.StartNew(() =>
					{
						var fileProcessor = new DirPairProcessor(dirPair, Settings, _logger);
						fileProcessor.Unpack(targetPath, _cancellationTokenSource.Token);
					}
					, _cancellationTokenSource.Token
				)
				.ContinueWith(LogErrorIfAny, TaskContinuationOptions.OnlyOnFaulted)
				.ContinueWith(o => Dispatcher.Invoke(new Action(() => { LayoutRoot.IsEnabled = true; })));
		}

		private void LogErrorIfAny(Task o)
		{
			Dispatcher.Invoke(new Action(() => { if (o.Exception != null) _logger.Log(false, o.Exception.ToString()); }));
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
			if (cultureCode == CoreResources.Culture.Name)
				return true;

			try
			{
				string oldFormat = CoreResources.Ui_Dialogs_SwitchLanguageFormat;

				CoreResources.Culture = CultureInfo.GetCultureInfo(cultureCode);
				string newFormat = CoreResources.Ui_Dialogs_SwitchLanguageFormat;
				string newLanguage = CoreResources.Common_Language;

				string question = 
					string.Format(oldFormat, newLanguage) + 
					Environment.NewLine + 
					string.Format(newFormat, newLanguage);
				
				if (MessageBox.Show(question, null, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					Settings.Culture = cultureCode;
					Settings.SaveToFile();
					Process.Start(System.Windows.Forms.Application.ExecutablePath);
					Application.Current.Shutdown();
				}
				else
				{
					CoreResources.Culture = CultureInfo.GetCultureInfo(Settings.Culture);
					return false;
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				CoreResources.Culture = CultureInfo.GetCultureInfo(Settings.Culture);
			}

			return true;
		}
	}
}
