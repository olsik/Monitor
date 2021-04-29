using Avalonia.Interactivity;
using Avalonia.Media;
using OxyPlot;
using OxyPlot.Series;

namespace Monitor
{
    public class MainViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        public MainViewModel()
        {
            // Create the plot model
            var tmp = new PlotModel();// { Title = "Simple example", Subtitle = "using OxyPlot" };

            // Create two line series (markers are hidden by default)
            var series1 = new LineSeries { Title = "Series 1", MarkerType = MarkerType.Circle };
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(10, 18));
            series1.Points.Add(new DataPoint(20, 12));
            series1.Points.Add(new DataPoint(30, 8));
            series1.Points.Add(new DataPoint(40, 15));

            var series2 = new LineSeries { Title = "Series 2", MarkerType = MarkerType.Square };
            series2.Points.Add(new DataPoint(0, 4));
            series2.Points.Add(new DataPoint(10, 12));
            series2.Points.Add(new DataPoint(20, 16));
            series2.Points.Add(new DataPoint(30, 25));
            series2.Points.Add(new DataPoint(40, 5));

            // Add the series to the plot model
            tmp.Series.Add(series1);
            tmp.Series.Add(series2);

            // Axes are created automatically if they are not defined

            tmp.IsLegendVisible = false;
            tmp.Series.RemoveAt(1);
            MainWindow.MAINVIEWMODEL = this;

            // Set the Model property, the INotifyPropertyChanged event will make the WPF Plot control update its content
            this.Model = tmp;
        }

        public PlotModel Model { get; private set; }

        bool[] status = new bool[4];
        public void btn_OnClick(object sender, Avalonia.Interactivity.RoutedEventArgs args)
        {
            Avalonia.Controls.Button? btn = sender as Avalonia.Controls.Button;
            int index = -1;
            int.TryParse(btn?.Tag?.ToString(), out index);
            if (index >= 0 && index < status.Length)
            {
                status[index] = !status[index];
                btn.Background = status[index] ? Brushes.LimeGreen : Brushes.Transparent;
            }
        }
    }
}