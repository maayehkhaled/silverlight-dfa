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

        public MainPage()
        {
            InitializeComponent();

            originalPlayboardColor = playboard.Background;

            loadAndDrawDFA(App.GetResourceStream(defaultFile).Stream);

            // TODO add startup animation
inputTextBox.Text = "0101";
        }

        private void openButton_Click(object sender, RoutedEventArgs args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
            {
                loadAndDrawDFA(openFileDialog.File.OpenRead());
                playboard.Background = originalPlayboardColor;
            }
            else
            {
                playboard.Background = new SolidColorBrush(Colors.Red);
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            animations = AnimationPlanner.createPlan(inputTextBox.Text, visualNodes[0]);

            //if (step == 0 && animations.Count > 0)
            //{
 
            //}

            step = 0;
            Storyboard first = animations[step];
            Debug.WriteLine("*** first is: " + first.GetValue(Storyboard.TargetNameProperty));
            first.Begin();
        }

        private void animationCompleted(object sender, EventArgs e)
        {
            step++;
            if (step >= animations.Count)
            {
                writeLog("All done.");
            }
            else
            {
                Storyboard next = animations[step];
                Debug.WriteLine("*** next is: " + next.GetValue(Storyboard.TargetNameProperty));
                next.Begin();
            }
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
        }
		
        private void playboard_Drop(object sender, DragEventArgs args)
        {
            if (args.Data != null)
            {
                FileInfo[] files = args.Data.GetData(DataFormats.FileDrop) as FileInfo[];
                writeLog(files.Length + " files dropped (only the first one will be read)...");

                loadAndDrawDFA(files[0].OpenRead());
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

        private void writeLog(string s) {
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

        private void speedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            //setExecutorDelay(e.NewValue);
        }

        private void loadAndDrawDFA(Stream stream)
        {
            playboard.Children.Clear();
            visualNodes = XmlParser.parse(stream);
            writeLog("File loaded.");
            drawer = new GraphDrawer(visualNodes, playboard);
            drawer.drawDFA();
            writeLog("Graph drawn.");

            foreach (VisualNode n in visualNodes)
            {
                n.getSrcAnimation().Completed += new EventHandler(animationCompleted);
                n.getDstAnimation().Completed += new EventHandler(animationCompleted);
            }
        }
    }
}
