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
using System.Windows.Shell;
using FigLeaf.Core.PasswordRules;
using Ookii.Dialogs.Wpf;

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
		public void Start()
		{
			LayoutRoot.IsEnabled = false;
			txtLog.Text = null;
			TaskbarItemInfo.ProgressValue = 0;
			TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Indeterminate;
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

		public void Stop()
		{
			LayoutRoot.IsEnabled = true;
			TaskbarItemInfo.ProgressValue = 1;
			TaskbarItemInfo.ProgressState = !IsActive 
				? TaskbarItemProgressState.Normal 
				: TaskbarItemProgressState.None;
		}

		public bool IsRunning
		{
			get { return !LayoutRoot.IsEnabled; }
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

		private void UpdateTarget(object sender, RoutedEventArgs ea)
		{
			_cancellationTokenSource = new CancellationTokenSource();
			_logger.Start();
			Task.Factory
				.StartNew(() =>
					{
						IEnumerable<DirPair> dirPairs = Settings.HasMultipleDirs
							? Settings.Dirs
							: Settings.Dirs.Take(1);

						var passwordRuleParser = new PasswordGenerator(Settings.PasswordRule, Settings.CustomPasswordRule, Settings.MasterPassword);
						var zip = new Zip(passwordRuleParser);
						var thumbnail = Settings.EnableThumbnails
							? new Thumbnail(Settings)
							: null;

						foreach (DirPair dirPair in dirPairs)
						{
							try
							{
								var fileProcessor = new DirPairProcessor(
									dirPair, Settings.ArchiveNameRule, Settings.ExcludeFigLeafDir, zip, thumbnail, _logger);
								fileProcessor.Pack(_cancellationTokenSource.Token, GetCleanTargetConfirm(Settings.ConfirmDelete));
								if (_cancellationTokenSource.IsCancellationRequested)
									break;
							}
							catch (Exception e)
							{
								LogException(e);
							}
						}
					}, 
					_cancellationTokenSource.Token
				)
				.ContinueWith(LogErrorIfAny, TaskContinuationOptions.OnlyOnFaulted)
				.ContinueWith(o => Dispatcher.Invoke(new Action(() => _logger.Stop())));
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

			var dlg = new VistaFolderBrowserDialog();
			dlg.Description = CoreResources.Ui_Dialogs_RestoreTarget;
			dlg.SelectedPath = dirPair.Source;
			dlg.ShowNewFolderButton = true;

			bool? result = dlg.ShowDialog();
			if (!result.HasValue || !result.Value)
				return;

			string targetPath = dlg.SelectedPath;

			_cancellationTokenSource = new CancellationTokenSource();
			_logger.Start();
			Task.Factory
				.StartNew(() =>
					{
						var passwordRuleParser = new PasswordGenerator(Settings.PasswordRule, Settings.CustomPasswordRule, Settings.MasterPassword);
						var zip = new Zip(passwordRuleParser);
						var fileProcessor = new DirPairProcessor(dirPair, Settings.ArchiveNameRule, false, zip, null, _logger);
						fileProcessor.Unpack(targetPath, _cancellationTokenSource.Token);
					}
					, _cancellationTokenSource.Token
				)
				.ContinueWith(LogErrorIfAny, TaskContinuationOptions.OnlyOnFaulted)
				.ContinueWith(o => Dispatcher.Invoke(new Action(() => _logger.Stop())));
		}

		private void LogErrorIfAny(Task o)
		{
			Dispatcher.Invoke(new Action(() => LogException(o.Exception)));
		}

		private void LogException(Exception e)
		{
			string message;
			var aggregate = e as AggregateException;
			if (aggregate == null)
			{
				message = e.Message;
			}
			else
			{
				message = aggregate.InnerExceptions != null 
					? string.Join(Environment.NewLine, aggregate.InnerExceptions.Select(ie => ie.Message).Distinct())
					: aggregate.InnerException != null
						? aggregate.InnerException.Message
						: aggregate.Message;
			}
			_logger.Log(false, string.Format(Core.Properties.Resources.Common_ErrorFormat, message));
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

		private void WindowActivated(object sender, EventArgs e)
		{
			// reset task bar button state
			if (!_logger.IsRunning)
				TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
		}
	}
}
