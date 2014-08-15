using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
	public partial class MainWindow : Window
	{
		public ISettings Settings { get; set; }

		public MainWindow()
		{
			InitializeComponent();

			Settings = new AppConfigSettings();
			DataContext = this;
		}

		private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Settings.Save();
		}
	}
}
