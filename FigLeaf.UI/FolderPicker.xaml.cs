using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

using Ookii.Dialogs.Wpf;

namespace FigLeaf.UI
{
	public partial class FolderPicker : UserControl
	{
		public FolderPicker()
		{
			InitializeComponent();
		}

		public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FolderPicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public static DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(FolderPicker), new PropertyMetadata(null));

		public string Text { get { return GetValue(TextProperty) as string; } set { SetValue(TextProperty, value); } }
		public string Description { get { return GetValue(DescriptionProperty) as string; } set { SetValue(DescriptionProperty, value); } }

		private void BrowseFolder(object sender, RoutedEventArgs e)
		{
			var dlg = new VistaFolderBrowserDialog();
			dlg.Description = Description;
			dlg.SelectedPath = Text;
			dlg.ShowNewFolderButton = true;

			bool? result = dlg.ShowDialog();
			if (result.HasValue && result.Value)
			{
				Text = dlg.SelectedPath;
				BindingExpression be = GetBindingExpression(TextProperty);
				if (be != null)
					be.UpdateSource();
			}
		}
	}
}
