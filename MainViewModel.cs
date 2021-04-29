using Avalonia.Interactivity;
using Avalonia.Media;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;

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

            tmp.Axes.Clear();
            tmp.Axes.Add(new LinearAxis { Minimum = 0, Maximum = 100, Position = AxisPosition.Bottom, StartPosition = 1, EndPosition = 0 });
            tmp.Axes.Add(new LinearAxis { Minimum = 0, Maximum = 100, Position = AxisPosition.Left, StartPosition = 1, EndPosition = 0 });

            // Set the Model property, the INotifyPropertyChanged event will make the WPF Plot control update its content
            this.Model = tmp;


            tmp.IsLegendVisible = false;
            tmp.Series.RemoveAt(1);
            MainWindow.MAINVIEWMODEL = this;
            InitSeries();
        }

        public PlotModel Model { get; private set; }

        bool[] status = new bool[4];
        OxyColor[] colors = new OxyColor[4];
        LineSeries[] series = new LineSeries[4];
        Random rnd = new Random();
        private void InitSeries()
        {
            colors[0] = OxyColors.GreenYellow;
            colors[1] = OxyColors.Blue;
            colors[2] = OxyColors.Fuchsia;
            colors[3] = OxyColors.Yellow;

            Model.Series.Clear();

            for (int i = 0; i < series.Length; i++)
            {
                series[i] = new LineSeries { Title = "Series " + i.ToString(),Color=colors[i] };
                Model.Series.Add(series[i]);
            }
        }
        private void AddRandomValue(int index)
        {
            series[index].Points.Add(new DataPoint(series[index].Points.Count, rnd.Next(0,100)));
            Model.InvalidatePlot(true);
        }
        public void btn_OnClick(object sender, Avalonia.Interactivity.RoutedEventArgs args)
        {
            Avalonia.Controls.Button? btn = sender as Avalonia.Controls.Button;
            int index = -1;
            int.TryParse(btn?.Tag?.ToString(), out index);
            if (index >= 0 && index < status.Length)
            {
                status[index] = !status[index];
                btn.Background = status[index] ? 
                    new SolidColorBrush(new Color(colors[index].A, colors[index].R, colors[index].G, colors[index].B))
                    : Brushes.Transparent;

                foreach (var a in Model.Axes)
                    a.Maximum -= 1;

                AddRandomValue(index);


            }
        }
    }
}