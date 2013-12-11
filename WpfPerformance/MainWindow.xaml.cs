using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace WpfPerformance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableDataSource<Point> dataSource = new ObservableDataSource<Point>();
        private PerformanceCounter cpuPerformance = new PerformanceCounter();
        private DispatcherTimer timer = new DispatcherTimer();
        private int i = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AnimatedPlot(object sender, EventArgs e)
        {
            cpuPerformance.CategoryName = "Processor";
            cpuPerformance.CounterName = "% Processor Time";
            cpuPerformance.InstanceName = "_Total";

            double x = i;
            double y = cpuPerformance.NextValue();

            Point point = new Point(x, y);
            dataSource.AppendAsync(base.Dispatcher, point);

            cpuUsageText.Text = String.Format("{0:0}%", y);
            i++;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            plotter.AddLineGraph(dataSource, Colors.Green, 2, "Percentage");
            timer.Interval = TimeSpan.FromMinutes(5);
            timer.Tick += new EventHandler(AnimatedPlot);
            timer.IsEnabled = true;
            plotter.Viewport.FitToView();
        }
    }
}
