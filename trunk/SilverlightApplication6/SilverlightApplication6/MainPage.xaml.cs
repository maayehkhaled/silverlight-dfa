using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using Microsoft.Expression.Controls;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Media.Animation;

namespace SilverlightApplication6
{
    public partial class MainPage : UserControl
    {
        private static Uri defaultFile = new Uri("testdata/HopcroftMotwaniUllman.xml", UriKind.Relative);

        private GraphDrawer drawer;
        private Brush originalPlayboardColor;
        private List<VisualNode> visualNodes;
        private List<Storyboard> animations;
        private int step;
        private string input;

        public MainPage()
        {
            InitializeComponent();

            originalPlayboardColor = playboard.Background;

            loadAndDraw(App.GetResourceStream(defaultFile).Stream);

            // TODO add startup animation
//inputTextBox.Text = "0101";
        }

        private void openButton_Click(object sender, RoutedEventArgs args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
            {
                loadAndDraw(openFileDialog.File.OpenRead());
                playboard.Background = originalPlayboardColor;
            }
            else
            {
                playboard.Background = new SolidColorBrush(Colors.Red);
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {

            //if (step == 0 && animations.Count > 0)
            //{
 
            //}

            if (inputTextBox.Text == null || inputTextBox.Text.Equals(""))
            {
                writeLog("Give me input!");
                return;
            }
            else if (!inputTextBox.Text.Equals(input))
            {
                input = inputTextBox.Text;
                step = 0;
                animations = AnimationPlanner.createPlan(input, visualNodes[0]);
                writeLog("Plan created.");
            }

            steplineSlider.Minimum = 0;
            steplineSlider.Maximum = animations.Count - 1;
            steplineSlider.Value = step;
            steplineSlider.SmallChange = 1;
            steplineSlider.LargeChange = 1;

            setControlsEnabled(false, false, false, true, false, false);

            writeLog("Starting animation...");
            Storyboard first = animations[step];
            Debug.WriteLine("*** first is: " + first.GetValue(Storyboard.TargetNameProperty));
            first.Begin();
        }

        private void animationCompleted(object sender, EventArgs e)
        {
            step++;
            if (step >= animations.Count)
            {
                step = 0;
                setControlsEnabled(true, true, true, false, true, true);
                writeLog("All done.");
            }
            else
            {
                Storyboard next = animations[step];
                steplineSlider.Value = step;
                Debug.WriteLine("*** next is: " + next.GetValue(Storyboard.TargetNameProperty));
                next.Begin();
            }
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            animations[step].Stop();
            setControlsEnabled(true, true, true, false, true, true);
        }
		
        private void playboard_Drop(object sender, DragEventArgs args)
        {
            if (args.Data != null)
            {
                FileInfo[] files = args.Data.GetData(DataFormats.FileDrop) as FileInfo[];
                writeLog(files.Length + " files dropped (only the first one will be read)...");

                loadAndDraw(files[0].OpenRead());
                playboard.Background = originalPlayboardColor;
            }
            else
            {
                playboard.Background = new SolidColorBrush(Colors.Red);
            }
        }

        private void playboard_DragOver(object sender, DragEventArgs e)
        {

        }

        private void playboard_DragEnter(object sender, DragEventArgs e)
        {
            playboard.Background = new SolidColorBrush(Colors.Green);
        }

        private void playboard_DragLeave(object sender, DragEventArgs e)
        {
            playboard.Background = originalPlayboardColor;
        }

        private void speedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (visualNodes != null)
            {
                foreach (VisualNode n in visualNodes)
                {
                    n.getSrcAnimation().SpeedRatio = e.NewValue;
                    n.getDstAnimation().SpeedRatio = e.NewValue;
                }
            }
        }

        private void steplineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            step = (int)e.NewValue;
            writeLog("Step choosen: " + step);
        }

        private void loadAndDraw(Stream stream)
        {
            playboard.Children.Clear();
            step = 0;

            visualNodes = XmlParser.parse(stream);
            foreach (VisualNode n in visualNodes)
            {
                n.getSrcAnimation().Completed += new EventHandler(animationCompleted);
                n.getDstAnimation().Completed += new EventHandler(animationCompleted);
            }
            writeLog("File loaded.");
            drawer = new GraphDrawer(visualNodes, playboard);
            drawer.drawDFA();
            writeLog("Graph drawn.");

            setControlsEnabled(true, true, true, false, true, true);
        }

        private void setControlsEnabled(bool enableInputTextBox,
            bool enableOpenButton, bool enableStartButton, bool enableStopButton,
            bool enableSpeedSlider, bool enableSteplineSlider)
        {
            inputTextBox.IsEnabled = enableInputTextBox;
            openButton.IsEnabled = enableOpenButton;
            startButton.IsEnabled = enableStartButton;
            stopButton.IsEnabled = enableStopButton;
            speedSlider.IsEnabled = enableSpeedSlider;
            steplineSlider.IsEnabled = enableSteplineSlider;
        }

        private void writeLog(string s)
        {
            if (logbox.Text.Length > 0)
            {
                logbox.Text += Environment.NewLine + s;
            }
            else
            {
                logbox.Text += s;
            }

            logbox.SelectionStart = logbox.Text.Length;
        }
    }
}
