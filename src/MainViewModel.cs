using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.Media;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Monitor
{
    public class MainViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        private List<IDataSource> DataSources = new List<IDataSource>();
        Timer aTimer = new Timer(500);
        private  int dataLength = 100;
        private int timePeriod_sec = 100;

        public MainViewModel()
        {
            // Create the plot model
            var tmp = new PlotModel();// { Title = "Simple example", Subtitle = "using OxyPlot" };

            // Create two line series (markers are hidden by default)
            // var series1 = new LineSeries { Title = "Series 1", MarkerType = MarkerType.Circle };
            // series1.Points.Add(new DataPoint(0, 0));
            // series1.Points.Add(new DataPoint(10, 18));
            // series1.Points.Add(new DataPoint(20, 12));
            // series1.Points.Add(new DataPoint(30, 8));
            // series1.Points.Add(new DataPoint(40, 15));

            // var series2 = new LineSeries { Title = "Series 2", MarkerType = MarkerType.Square };
            // series2.Points.Add(new DataPoint(0, 4));
            // series2.Points.Add(new DataPoint(10, 12));
            // series2.Points.Add(new DataPoint(20, 16));
            // series2.Points.Add(new DataPoint(30, 25));
            // series2.Points.Add(new DataPoint(40, 5));

            // // Add the series to the plot model
            // tmp.Series.Add(series1);
            // tmp.Series.Add(series2);

            // tmp.Series.RemoveAt(1);
            // Axes are created automatically if they are not defined

            tmp.Axes.Clear();
            //tmp.Axes.Add(new LinearAxis { Minimum = 0, Maximum = 100, Position = AxisPosition.Bottom, StartPosition = 1, EndPosition = 0 });
            tmp.Axes.Add(new LinearAxis { Minimum = 0, Maximum = timePeriod_sec, Position = AxisPosition.Bottom, StartPosition = 1, EndPosition = 0 });
            // tmp.Axes.Add(new LinearAxis { Minimum = 0, Maximum = 100, Position = AxisPosition.Left, StartPosition = 1, EndPosition = 0 });
            //tmp.Axes.Add(new LinearAxis { Minimum = 0, Maximum = 100, Position = AxisPosition.Left });
            tmp.Axes.Add(new LinearAxis { Minimum = 0, Position = AxisPosition.Left });

            // Set the Model property, the INotifyPropertyChanged event will make the WPF Plot control update its content
            this.Model = tmp;


            tmp.IsLegendVisible = false;
            MainWindow.MAINVIEWMODEL = this;
            InitSeries();
            CreateTimer();
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

            //for (int i = 0; i < series.Length; i++)
            //    DataSources.Add(new DataSource());
            DataSources.Add(new NetworkDownDataSource());
            DataSources.Add(new NetworkUpDataSource());

            for (int i = 0; i < series.Length; i++)
            {
                series[i] = new LineSeries { Title = "Series " + i.ToString(), Color = colors[i] };
                Model.Series.Add(series[i]);
            }
        }
        private void GetDataAndFillSeries()
        {
            foreach (var s in series)
                s.Points.Clear();

            for (int i = 0; i < DataSources.Count; i++)
            {
                int[] curData = DataSources[i].Data;
                for (int d = 0; d < curData.Length; d++)
                    series[i].Points.Add(new DataPoint(d, curData[d]));
            }
        }

        private void CreateTimer()
        {
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            aTimer.Enabled = false;
            GetDataAndFillSeries();
            Dispatcher.UIThread.InvokeAsync(UpdatePlot);
            // Model.InvalidatePlot(true);
            aTimer.Enabled = true;
        }
        void UpdatePlot()
        {
            Model.InvalidatePlot(true);
        }
        public void btn_OnClick(object sender, Avalonia.Interactivity.RoutedEventArgs args)
        {
            Avalonia.Controls.Button? btn = sender as Avalonia.Controls.Button;
            int index = -1;
            int.TryParse(btn?.Tag?.ToString(), out index);
            if (btn != null && index >= 0 && index < status.Length)
            {
                status[index] = !status[index];
                btn.Background = status[index] ?
                    new SolidColorBrush(new Color(colors[index].A, colors[index].R, colors[index].G, colors[index].B))
                    : Brushes.Transparent;
                series[index].Color = status[index] ? colors[index] : OxyColors.Transparent;
                // foreach (var a in Model.Axes)
                //     a.Maximum -= 1;

                // AddRandomValue(index);

                if (flag)
                {
                    timePeriod_sec -= 20;
                    flag = timePeriod_sec > 20;
                }

                else
                    timePeriod_sec += 20;
                dataLength = timePeriod_sec;
                Model.Axes[0].Maximum = timePeriod_sec;

                for (int i = 0; i < DataSources.Count; i++)
                    DataSources[i].SetParams(dataLength, timePeriod_sec);

            }
        }
        bool flag = true;
    }
}