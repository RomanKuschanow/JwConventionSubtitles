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


namespace JwConventionSubtitles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var reader = new FileReader();
            var parser = new VttParser();

            var lines = reader.ReadLines(@"C:\CO-r22_E_01.vtt");
            var frames = parser.Parse(lines.ToList()).ToArray();
            var allFrames = frames
                .Select((item, index) => (item, index))
                .ToArray();

            var results = allFrames
                .Skip(2)
                .Zip(allFrames.Skip(3).Take(allFrames.Length - 5))
                .Select(pair => new
                {
                    distance = pair.Second.item.StartTime - pair.First.item.EndTime,
                    frame1Time = pair.First.item.StartTime,
                    frame2Time = pair.Second.item.StartTime, 
                    frame1Text = string.Join(" ",
                        frames[(pair.First.index - 2)..(pair.First.index)]
                            .SelectMany(r => r.Lines))
                        .ReplaceLineEndings(" "),
                    frame2Text = string.Join(" ",
                        frames[(pair.Second.index)..(pair.Second.index + 2)]
                            .SelectMany(r => r.Lines))
                        .ReplaceLineEndings(" ")
                })
                .OrderByDescending(r => r.distance)
                .Where(r => r.distance > TimeSpan.FromSeconds(2))
                .ToList();

            DataGrid1.ItemsSource = results;
        }
    }
}
