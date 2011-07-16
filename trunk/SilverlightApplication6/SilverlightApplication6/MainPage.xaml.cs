﻿using System;
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

namespace SilverlightApplication6
{
    public partial class MainPage : UserControl
    {
        private static Uri defaultFile = new Uri("testdata/HopcroftMotwaniUllman.xml", UriKind.Relative);
        private static string logMessages;

        private GraphDrawer drawer;
        private Brush originalPlayboardColor;

        private BackgroundWorker bw = new BackgroundWorker();

        public MainPage()
        {
            InitializeComponent();

            originalPlayboardColor = playboard.Background;

            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;


            //DummyTest dt = new DummyTest();
			
            //dt.drawDummyDFA(playboard);

            List<Node> nodes = XmlParser.parse(App.GetResourceStream(defaultFile).Stream);
            writeLog("Default file loaded.");

            drawer = new GraphDrawer(nodes, playboard);
            drawer.drawDFA();
            writeLog("Graph drawn.");
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {

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

        public void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                writeLog("Stopped.");
            }

            else if (!(e.Error == null))
            {
                writeLog("Error: " + e.Error.Message);
            }

            else
            {
                writeLog("Done.");
            }
        }

        public void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.steplineSlider.Value = e.ProgressPercentage;
        }
    }
}
