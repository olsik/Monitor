using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Monitor
{
    public class MainWindow : Window
    {
        public static MainViewModel? MAINVIEWMODEL;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public void btn_OnClick(object sender, Avalonia.Interactivity.RoutedEventArgs args)
        {
            MAINVIEWMODEL?.btn_OnClick(sender, args);
        }
    }
}
