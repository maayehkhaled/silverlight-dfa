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

namespace SilverlightApplication6
{
    public partial class MainPage : UserControl
    {
        private static Uri defaultFile = new Uri("testdata/HopcroftMotwaniUllman.xml", UriKind.Relative);

        private GraphDrawer drawer;
        private Brush originalPlayboardColor;

        public MainPage()
        {
            InitializeComponent();
            originalPlayboardColor = playboard.Background;
            //DummyTest dt = new DummyTest();
			
            //dt.drawDummyDFA(playboard);

            List<Node> nodes = XmlParser.parse(App.GetResourceStream(defaultFile).Stream);
            writeLog("Default file loaded.");
            Debug.WriteLine("*** nodes:");
            foreach (Node n in nodes)
            {
                Debug.WriteLine("*** node: " + n.nodeLabel + ", isEnd: " + n.isEnd + ", x: " +  n.x + ", y: " + n.y);
                foreach (Tuple<Node, string> t in n.adjacent)
                {
                    Debug.WriteLine("*** to node: " + ((Node) t.Item1).nodeLabel + ", with: " + t.Item2);
                }
            }

            drawer = new GraphDrawer(nodes, playboard);
            drawer.drawDFA();
            writeLog("Graph drawn.");
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
			/*
            Ellipse e1 = new Ellipse();
            e1.Height = 40;
            e1.Width = 40;
            e1.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x3D, 0xE3, 0x18));
            playboard.Children.Add(e1);
            
            double y1 = 100.0;
            double x1 = 100.0;
            e1.DataContext = "inf loop";
            e1.SetValue(Canvas.TopProperty, y1);
            e1.SetValue(Canvas.LeftProperty, x1);

            Ellipse e2 = new Ellipse();
            e2.Height = 40;
            e2.Width = 40;
            e2.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x3D, 0xE3, 0x18));
            playboard.Children.Add(e2);
            double y2 = 200.0;
            double x2 = 200.0;
            e2.SetValue(Canvas.TopProperty, y2);
            e2.SetValue(Canvas.LeftProperty, x2);

            LineArrow a = new LineArrow();
            a.Height = Math.Abs(y1 -y2) - e2.Width/4 ;
            a.Width = Math.Abs(x1-x2) -e2.Height/4 ;
            a.Stroke = new SolidColorBrush(Colors.Black);
            a.StrokeThickness = 2;
            a.Opacity = 0.5;

            playboard.Children.Add(a);
            a.SetValue(Canvas.TopProperty, y1 + e1.Height /1);
            a.SetValue(Canvas.LeftProperty, x1 + e1.Width / 1);
			*/ 
        }
		
        private void playboard_Drop(object sender, DragEventArgs args)
        {
            if (args.Data != null)
            {
                FileInfo[] files = args.Data.GetData(DataFormats.FileDrop) as FileInfo[];
                writeLog(files.Length + " files dropped (only the first one will be read)...");

                //try
                //{
                    List<Node> nodes = XmlParser.parse(files[0]);
                    writeLog("File loaded.");
                    drawer = new GraphDrawer(nodes, playboard);
                    drawer.drawDFA();
                    writeLog("Graph drawn.");
                    playboard.Background = originalPlayboardColor;
                //}
                //catch (Exception e)
                //{
                //    writeLog(e.ToString());
                //    playboard.Background = new SolidColorBrush(Colors.Red);
                //}
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

        private void openButton_Click(object sender, RoutedEventArgs args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
            {
                //try
                //{
                    List<Node> nodes = XmlParser.parse(openFileDialog.File);
                    writeLog("File loaded.");
                    drawer = new GraphDrawer(nodes, playboard);
                    drawer.drawDFA();
                    writeLog("Graph drawn.");
                    playboard.Background = originalPlayboardColor;
                //}
                //catch (Exception e)
                //{
                //    writeLog(e.ToString());
                //    playboard.Background = new SolidColorBrush(Colors.Red);
                //}
            }
            else
            {
                playboard.Background = new SolidColorBrush(Colors.Red);
            }
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
    }
}
