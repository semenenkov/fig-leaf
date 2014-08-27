using System.Windows;
using System.Windows.Controls;

namespace FigLeaf.UI
{
	/// <summary>
	/// Interaction logic for QuestionTip.xaml
	/// </summary>
	public partial class QuestionTip : UserControl
	{
		public QuestionTip()
		{
			InitializeComponent();
		}

		public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(QuestionTip), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public string Text { get { return GetValue(TextProperty) as string; } set { SetValue(TextProperty, value); } }

		private void ShowText(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(Text))
				MessageBox.Show(Text);
		}
	}
}
