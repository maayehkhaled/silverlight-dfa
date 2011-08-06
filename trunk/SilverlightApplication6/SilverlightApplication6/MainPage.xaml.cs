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
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace SilverlightApplication6
{
    public partial class MainPage : UserControl
    {
        private static Uri defaultFile = new Uri("testdata/HopcroftMotwaniUllman.xml", UriKind.Relative);

        private GraphDrawer drawer;
        private Brush originalPlayboardColor;
        private Regex inputRegex;
        private List<VisualNode> visualNodes;
        private List<VisualInput> visualInput = new List<VisualInput>();
        private List<Storyboard> animations;
        private int step;

        public MainPage()
        {
            InitializeComponent();

            originalPlayboardColor = playboard.Background;
            //inputTextBox.TextChanged += new TextChangedEventHandler(inputChanged);
            //steplineSlider.Minimum = 0;
            //steplineSlider.SmallChange = 1;
            //steplineSlider.LargeChange = 1;
            for (int i = 0; i < inputTextBox.MaxLength; i++)
            {
                VisualInput vi = new VisualInput();
                double fromX = 10.0 + vi.getGrid().Width * i;
                double fromY = 10.0;
                vi.setLocation(new EPoint(fromX, fromY));

                visualInput.Add(vi);
                //playboard.Children.Add(vi.getGrid());
                Debug.WriteLine("*** added visual input: fromX: " + fromX + ", fromY: " + fromY);
            }
            setControlsEnabled(false, true, false, false, false, false);

            writeLog("Loading default file: " + defaultFile.OriginalString);
            loadAndDraw(App.GetResourceStream(defaultFile).Stream);

            // TODO add startup animation
// only for debug
//inputTextBox.Text = "0101";
//for (int i = 0; i < 10; i++)
//{
//    VisualInput vi = visualInput[i];
//    playboard.Children.Add(vi.getGrid());
//    vi.getGrid().SetValue(Canvas.LeftProperty, 10.0);
//    vi.getGrid().SetValue(Canvas.TopProperty, 10.0);
//    Debug.WriteLine("*** added visual input to grid: " + i);
//}
        }

        private void openButton_Click(object sender, RoutedEventArgs args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    loadAndDraw(openFileDialog.File.OpenRead());
                }
                catch (Exception e)
                {
                    playboard.Background = new SolidColorBrush(Colors.Red);
                    writeLog("Unexpected error: " + e.Message);
                    return;
                }
            }
            else
            {
                playboard.Background = new SolidColorBrush(Colors.Red);
            }
        }

        private void inputChanged(object sender, TextChangedEventArgs e)
        {
            if (inputTextBox.Text.Length == inputTextBox.MaxLength)
            {
                writeLog("Maximum input length reached.");
            }

            if (inputRegex.IsMatch(inputTextBox.Text))
            {
                step = 0;
                animations = AnimationPlanner.createPlan(inputTextBox.Text, visualNodes[0], visualInput);
                writeLog("Plan created.");

                steplineSlider.Maximum = animations.Count - 1;
                steplineSlider.Value = step;

                setControlsEnabled(true, true, true, false, true, true);
            }
            else if (inputTextBox.Text.Equals(""))
            {
                setControlsEnabled(true, true, false, false, true, false);
            }
            else
            {
                setControlsEnabled(true, true, false, false, true, false);
                writeLog("Illegal input: " + inputTextBox.Text);
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            setControlsEnabled(false, false, false, true, false, false);

            writeLog("Starting animation...");
            Storyboard first = animations[step];
            Debug.WriteLine("*** first is: " + first.GetValue(Storyboard.TargetNameProperty));
            first.Begin();
        }

        private void animationCompleted(object sender, EventArgs e)
        {
            Debug.WriteLine("*** animationCompleted()");

            // reset the animation to it's start state
            Storyboard sb = sender as Storyboard;
            String s = Storyboard.GetTargetName(sb);
            if (s != null && s.StartsWith("visualInput"))
            {
                //sb.Stop();
            }
            else
            {
                sb.Stop();
            }

            step++;
            if (step >= animations.Count)
            {
                step = 0;
                foreach (Storyboard a in animations)
                {
                    a.Stop();
                }
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
            writeLog("Stopped.");
            setControlsEnabled(true, true, true, false, true, true);
        }
		
        private void playboard_Drop(object sender, DragEventArgs args)
        {
            if (args.Data != null)
            {
                FileInfo[] files = args.Data.GetData(DataFormats.FileDrop) as FileInfo[];
                writeLog(files.Length + " files dropped (only the first one will be read)...");

                try
                {
                    loadAndDraw(files[0].OpenRead());
                }
                catch (Exception e)
                {
                    playboard.Background = new SolidColorBrush(Colors.Red);
                    writeLog("Unexpected error: " + e.Message);
                    return;
                }
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
                    n.getAcceptedAnimation().SpeedRatio = e.NewValue;
                    n.getRejectedAnimation().SpeedRatio = e.NewValue;
                    foreach (Tuple<VisualNode, string> t in n.adjacenceList)
                    {
                        n.getDstEdge(t.Item2).getAnimation().SpeedRatio = e.NewValue;
                    }
                }
            }
            foreach (VisualInput vi in visualInput)
            {
                vi.getConsumeAnimation().SpeedRatio = e.NewValue;
                vi.getFadeInAnimation().SpeedRatio = e.NewValue;
            }
            writeLog("Speed altered to: " + e.NewValue);
        }

        private void steplineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            step = (int)e.NewValue;
            writeLog("Step: " + step);
        }

        private void loadAndDraw(Stream stream)
        {
            playboard.Children.Clear();
            step = 0;
            inputTextBox.Text = "";

            // TODO a better place for this!?
            foreach (VisualInput vi in visualInput)
            {
                playboard.Children.Add(vi.getGrid());
                Debug.WriteLine("*** added visual input to grid");
            }

            List<string> inputAlphabet;
            visualNodes = XmlParser.parse(stream, out inputAlphabet);
            string pattern = @"^[";
            foreach (string symbol in inputAlphabet)
            {
                pattern += symbol;
            }
            pattern += "]+$";
            Debug.WriteLine("*** pattern: " + pattern);
            inputRegex = new Regex(pattern);
            playboard.Background = originalPlayboardColor;
            writeLog("File loaded.");

            drawer = new GraphDrawer(visualNodes, playboard);
            drawer.drawDFA();
            writeLog("Graph drawn.");
            
            foreach (VisualNode n in visualNodes)
            {
                n.getSrcAnimation().Completed += new EventHandler(animationCompleted);
                n.getDstAnimation().Completed += new EventHandler(animationCompleted);
                n.getAcceptedAnimation().Completed += new EventHandler(animationCompleted);
                n.getRejectedAnimation().Completed += new EventHandler(animationCompleted);
                n.getSrcAnimation().SpeedRatio = speedSlider.Value;
                n.getDstAnimation().SpeedRatio = speedSlider.Value;
                n.getAcceptedAnimation().SpeedRatio = speedSlider.Value;
                n.getRejectedAnimation().SpeedRatio = speedSlider.Value;

                foreach (Tuple<VisualNode, string> t in n.adjacenceList)
                {
                    n.getDstEdge(t.Item2).getAnimation().Completed += new EventHandler(animationCompleted);
                    n.getDstEdge(t.Item2).getAnimation().SpeedRatio = speedSlider.Value;
                    //Debug.WriteLine("*** eventhandler added for node: " + n.getLabelText() + ", edge: " + t.Item2);
                }
            }
            foreach (VisualInput vi in visualInput)
            {
                vi.getConsumeAnimation().Completed += new EventHandler(animationCompleted);
                vi.getConsumeAnimation().SpeedRatio = speedSlider.Value;
                vi.getFadeInAnimation().Completed += new EventHandler(animationCompleted);
                vi.getFadeInAnimation().SpeedRatio = speedSlider.Value;
            }
            //writeLog("Event handlers added.");

            setControlsEnabled(true, true, false, false, true, false);
            writeLog("Ready.");
        }

        private void setControlsEnabled(bool enableInputTextBox, bool enableOpenButton,
            bool enableStartButton, bool enableStopButton,
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
            if (logbox != null)
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
}
