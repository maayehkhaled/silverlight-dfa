using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SilverlightApplication6
{
    public class Executor
    {
        private VisualNode startNode;
        private int delay;

        private string remainingInput;
        private int inputLength;

        public void doWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while (!worker.CancellationPending && remainingInput.Length > 0)
            {

                if (remainingInput.Length > 1)
                {
                    remainingInput = remainingInput.Remove(0, 1);
                }
                else
                {
                    remainingInput = "";
                }

                int processed = inputLength - remainingInput.Length;
                Debug.WriteLine("*** remaining: " + remainingInput.Length + " processed: " + processed);
                worker.ReportProgress(processed);

                System.Threading.Thread.Sleep(delay);
            }

            e.Cancel = true;
        }

        public void setStartNode(VisualNode startNode)
        {
            this.startNode = startNode;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void setDelay(int delay)
        {
            this.delay = delay;
        }

        public void setInput(string input)
        {
            remainingInput = input;
            inputLength = input.Length;
        }
    }
}
