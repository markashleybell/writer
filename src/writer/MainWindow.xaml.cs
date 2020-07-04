using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace writer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var assembly = typeof(MainWindow).Assembly;

            InitializeComponent();

            Editor.WordWrap = true;
            Editor.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));

            Editor.FontFamily = new FontFamily("Consolas");
            Editor.FontSize = 16;

            Editor.TextArea.TextView.BackgroundRenderers.Insert(1, new ColorBackgroundRenderer(Editor));

            Editor.TextArea.SelectionCornerRadius = 0;
            Editor.TextArea.SelectionBorder = null;

            SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e) =>
            CalculateMargin();

        private void CalculateMargin()
        {
            const int textWidth = 700;

            var margin = (ActualWidth - textWidth) / 2;

            Editor.Padding = new Thickness(margin, 0, margin, 0);
        }
    }
}
