using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        FileReader reader = new FileReader();
        WebFileReader webReader = new WebFileReader();
        VttParser vttParser = new VttParser();
        ConventionProgramParser programParser = new ConventionProgramParser();
        SpeechConverter speechConverter = new SpeechConverter();
        IEnumerable<SpeechWithText> speechesWithText = new List<SpeechWithText>();

        public ObservableCollection<Button> SpeechButtons { get; set; } = new ObservableCollection<Button>();

        string vttURL = "", programPath = "";

        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            itemControl.DataContext = this;

            #region 

            /*var allFrames = frames
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

            DataGrid1.ItemsSource = results;*/

            #endregion
        }

        private void Get_URL(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            vttURL = textBox.Text;
        }
        
        private void SelectProgram_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "Document";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                programPath = dlg.FileName;
                if (vttURL != "") convert.IsEnabled = true;
            }
        }

        private async void Convert_Click(object sender, RoutedEventArgs e)
        {

            var vttLines = await webReader.ReadLines(vttURL);
            var programLines = await reader.ReadLines(programPath);
            var frames = vttParser.Parse(vttLines.ToList());
            var speechesFromProgram = programParser.Parse(programLines.ToList());
            speechesWithText = speechConverter.Convert(frames.ToList(), speechesFromProgram.ToList());

            SpeechButtons.Clear();

            foreach (SpeechWithText speech in speechesWithText)
            {
                Button button = new();
                button.Content = speech.Name;
                button.Margin = new Thickness(4);
                button.Click += Speech_Click;
                SpeechButtons.Add(button);
            }
        }

        private void Speech_Click(object sender, RoutedEventArgs e)
        {
            SpeechWithText speechWithText = new SpeechWithText("", "");

            foreach (var item in speechesWithText)
            {
                if (item.Name == (sender as Button).Content.ToString())
                {
                    speechWithText = item;
                    break;
                }
            }

            Clipboard.SetText(speechWithText.Text);
        }
    }
}
