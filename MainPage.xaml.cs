#define DEBUG

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
		
        private static Uri defaultFile = 
			new Uri("testdata/ModifiedHMU.xml", UriKind.Relative);
        private static Uri hmup60File = 
			new Uri("testdata/HopcroftMotwaniUllman.xml", UriKind.Relative);
		private static Uri sipser = 
			new Uri("testdata/Sipser.xml", UriKind.Relative);

        private Dictionary<string, Uri> builtinExamples = new Dictionary<string, Uri>();

        private GraphDrawer drawer;
        private Brush originalPlayboardColor;

		/** reg to check legal input */
        private Regex inputRegex;

		/** list of all presentations of states of DFA */
        //private List<VisualNode> visualNodes;
		private List<VisualAnimationNode> visualAnimationNodes;

		/** list of presetation of the input string */
        private List<VisualAnimationInput> visualInput = new List<VisualAnimationInput>();

		/** list if all storybords, which present the moving of the input characters */
        private List<Storyboard> animations;

		/* simulate the DFA. It creates necessery animations and 
		 * saves it in the list animations over a instance of AminationPlanner.
		 */
		private DFA<VisualAnimationNode,VisualAnimationInput> executor;
		
		/* factory, which generates animation */
		private AnimationInputFactory<VisualAnimationNode,VisualAnimationInput>
			factory;
	
		/* planer, which saves all animations of a process of an input*/
		private AnimationPlanner<VisualAnimationNode,VisualAnimationInput>
			planner;

		/* what is this? */
        private int step;

		/* what is this? */
        private bool flowingInput = false;

        public MainPage()
        {
            InitializeComponent();

            originalPlayboardColor = playboard.Background;
			
			builtinExamples.Add("Example 1", defaultFile);
            builtinExamplesComboBox.Items.Add("Example 1");
			//builtinExamplesComboBox.SelectedIndex = 0;

            builtinExamples.Add("Example 2", hmup60File);
            builtinExamplesComboBox.Items.Add("Example 2");
            //builtinExamplesComboBox.SelectedIndex = 1;

			builtinExamples.Add("Example 3", sipser);
			builtinExamplesComboBox.Items.Add("Example 3");
			builtinExamplesComboBox.SelectedIndex = 2;

			setControlsEnabled(false, true, false, false, false, false, false);

            writeLog("Loading default file: " + sipser.OriginalString);
            loadAndDrawDFA(App.GetResourceStream(sipser).Stream);

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

		private void makePresentationOfInputString()
		{
			Debug.WriteLine("call makePresentationOfInputString");
			// be sure that visualInput empty
			visualInput.Clear();

			//for (int i = 0; i < inputTextBox.MaxLength; i++)
			for (int i = 0; i < inputTextBox.Text.Length; i++)
			{
				VisualAnimationInput vi = new VisualAnimationInput();
				double fromX = 10.0 + vi.getGrid().Width * i;
				double fromY = 10.0;
				vi.setLocation(new EPoint(fromX, fromY));

				visualInput.Add(vi);
				
				Debug.WriteLine("*** added visual input: fromX: " + fromX + ", fromY: " + fromY);
				vi.setLabelText(inputTextBox.Text[i].ToString());
				playboard.Children.Add(vi.getGrid());
			}
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
                    loadAndDrawDFA(openFileDialog.File.OpenRead());
                }
                catch (Exception e)
                {
                    playboard.Background = new SolidColorBrush(Colors.Red);
                    writeLog("Unexpected error: " + e.Message);
                    return;
                }
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
                writeLog("input changed.");
				//Do this things  later
                setControlsEnabled(true, true, true, false, true, true, false);
            }
            else if (inputTextBox.Text.Trim().Equals(""))
            {
                setControlsEnabled(true, true, false, false, true, false, true);	
            }
            else
            {
                // -- i think this might never be reached
				// -- oh ja this has been reached
                setControlsEnabled(true, true, false, false, true, false, false);
                writeLog("Illegal input: " + inputTextBox.Text);
            }
        }
		

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            setControlsEnabled(false, false, false, true, false, false, false);
            writeLog("Starting animation...");
			
			// make a new AnimationPlaner here
			String inputString = inputTextBox.Text;
			makePresentationOfInputString();
			planner.clean();

			string result = executor.Simulate(inputString,planner,factory);
			planner.showActionInDebug();
			animations = planner.getAnimations();
			
			
			// combine the storyboards in animations
			 
			foreach (Storyboard s in animations )
			{
				s.Completed += new EventHandler(animationCompleted);
			}
            Storyboard first = animations[step];
            //Debug.WriteLine("*** first is: " + first.GetValue(Storyboard.TargetNameProperty));
            first.Begin();
			writeLog(result);
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
                setControlsEnabled(true, true, true, false, true, true, false);
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
            setControlsEnabled(true, true, true, false, true, true, false);
        }
		
        private void playboard_Drop(object sender, DragEventArgs args)
        {
            if (args.Data != null)
            {
                FileInfo[] files = args.Data.GetData(DataFormats.FileDrop) as FileInfo[];
                writeLog(files.Length + " files dropped (only the first one will be read)...");

                try
                {
                    loadAndDrawDFA(files[0].OpenRead());
                }
                catch (Exception e)
                {
                    playboard.Background = new SolidColorBrush(Colors.Red);
                    writeLog("Unexpected error: " + e.Message);
                    return;
                }
            }
        }

        

        private void speedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (visualAnimationNodes != null)
            {
                foreach (VisualAnimationNode n in visualAnimationNodes)
                {
                    n.getSrcAnimation().SpeedRatio = e.NewValue;
                    n.getDstAnimation().SpeedRatio = e.NewValue;
                    n.getAcceptedAnimation().SpeedRatio = e.NewValue;
                    n.getRejectedAnimation().SpeedRatio = e.NewValue;
                    foreach (Tuple<VisualNode, string> t in n.adjacenceList)
                    {
                        if (!t.Item2.Contains('|'))
                        {
                            n.getDstEdge(t.Item2).getAnimation().SpeedRatio = e.NewValue;
                        }
                        else
                        {
                            string[] symbolS = t.Item2.Split('|');
                            for (int i = 0; i < symbolS.Length; i++)
                            {
                                n.getDstEdge(symbolS[i]).getAnimation().SpeedRatio = e.NewValue;
                            }
                        }
                    }
                }
            }
            foreach (VisualAnimationInput vi in visualInput)
            {
                vi.getConsumeAnimation().SpeedRatio = e.NewValue;
                vi.getFadeInAnimation().SpeedRatio = e.NewValue;
                vi.getSearchAnimation().SpeedRatio = e.NewValue;
            }
            writeLog("Speed altered to: " + e.NewValue);
        }

        

		/// <summary>
		/// load the description of a DFA in a XML file and draw it on playboard
		/// Side effect: clear the playboard, all elements of his childeren are removed
		/// </summary>
		/// <param name="stream"></param>
        private void loadAndDrawDFA(Stream stream)
        {
            playboard.Children.Clear();
            step = 0;
            inputTextBox.Text = "";

			/* now begin to make a draw of DFA */
            List<string> inputAlphabet;
            visualAnimationNodes = XmlParser.parse(stream, out inputAlphabet);
			/* draw DFA on playboard */
			drawer = new GraphDrawer(visualAnimationNodes, playboard);
            drawer.drawDFA();
            writeLog("Graph drawn.");
			

			/* crate pattern, it will be used later to check if input legal or not */
            string pattern = @"^[";
            foreach (string symbol in inputAlphabet)
            {
                pattern += symbol;
            }
            pattern += "]+$";
            Debug.WriteLine("*** pattern: " + pattern);
            inputRegex = new Regex(pattern);

			/* set some optical signals to note the user that DFA drawed success */
            playboard.Background = originalPlayboardColor;
            //writeLog("File loaded.");

			/* cannot downcasting in C# with generic type, so I use this collection
			 * as a dummy collection, which saves VisualAnimationNode as VisualNode.
			 * It will be used later for DFA*/
			List<VisualNode> visualNodes = new List<VisualNode>();
			writeLog("creating DFA ...");

			/* set EventHandler to each stated */
            foreach (VisualAnimationNode n in visualAnimationNodes)
            {	
				// add a new reference to n
				visualNodes.Add(n);
            }

            //writeLog("Event handlers added.");
			// make a new animationfactory
			factory = new AnimationInputFactory<VisualAnimationNode,VisualAnimationInput>();
			factory.setInputChars(visualInput);
			factory.setNodes(visualAnimationNodes);
			/* planer here */
			planner = new AnimationPlanner<VisualAnimationNode,VisualAnimationInput>();
			/* create DFA */
			executor = new DFA<VisualAnimationNode, VisualAnimationInput>(visualNodes);
			
            setControlsEnabled(true, true, false, false, true, false, true);
            writeLog("Ready.");
        }

		/// <summary>
		/// control the GUI elements
		/// </summary>
		/// <param name="enableInputTextBox"></param>
		/// <param name="enableOpenButton"></param>
		/// <param name="enableStartButton"></param>
		/// <param name="enableStopButton"></param>
		/// <param name="enableSpeedSlider"></param>
		/// <param name="enableSteplineSlider"></param>
		/// <param name="enableInputCheckBox"></param>
        private void setControlsEnabled(bool enableInputTextBox, bool enableOpenButton,
            bool enableStartButton, bool enableStopButton,
            bool enableSpeedSlider, bool enableSteplineSlider,
            bool enableInputCheckBox)
        {
            inputTextBox.IsEnabled = enableInputTextBox;

            openButton.IsEnabled = enableOpenButton;
            startButton.IsEnabled = enableStartButton;
            stopButton.IsEnabled = enableStopButton;

            speedSlider.IsEnabled = enableSpeedSlider;
            
            if (enableSteplineSlider && flowingInput)
            {
                steplineSlider.IsEnabled = false;
            }
            else
            {
                steplineSlider.IsEnabled = enableSteplineSlider;
            }
            flowingInputCheckBox.IsEnabled = enableInputCheckBox;
            builtinExamplesComboBox.IsEnabled = enableOpenButton; // same like the open button
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

        private void flowingInput_Checked(object sender, RoutedEventArgs e)
        {
            flowingInput = true;
            writeLog("Flowing input disables stepline.");
        }

        private void flowingInput_Unchecked(object sender, RoutedEventArgs e)
        {
            flowingInput = false;
            writeLog("Stepline enabled.");
        }

        private void builtinExamplesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //try
            //{
                string key = ((ComboBox)sender).SelectedItem.ToString();
                loadAndDrawDFA(App.GetResourceStream(builtinExamples[key]).Stream);
                writeLog("Example changed.");
            //}
            //catch (Exception ex)
            //{
            //    playboard.Background = new SolidColorBrush(Colors.Red);
            //    writeLog("Unexpected error: " + ex.Message);
            //    return;
            //}
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

		private void steplineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			step = (int)e.NewValue;
			writeLog("Step: " + step);
		}
	}
}
