using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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

            // FullScreen();

            PreviewKeyDown +=
                (s, e) => {
                    if (e.Key == Key.F11)
                    {
                        if (WindowStyle != WindowStyle.SingleBorderWindow)
                        {
                            Windowed();
                        }
                        else
                        {
                            FullScreen();
                        }
                    }
                };

            Editor.WordWrap = true;
            Editor.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));

            Editor.FontFamily = new FontFamily("Consolas");
            Editor.FontSize = 16;

            // Editor.TextArea.TextView.BackgroundRenderers.Insert(1, new WriterBackgroundRenderer(Editor));

            Editor.TextArea.TextView.LineTransformers.Insert(0, new WriterColorizingTransformer(Editor));

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

        private void FullScreen()
        {
            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            Topmost = true;
        }

        private void Windowed()
        {
            ResizeMode = ResizeMode.CanResize;
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Normal;
            Topmost = false;
        }
    }
}
