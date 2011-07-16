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

namespace SilverlightApplication6
{
    public class Executor
    {
        private string remainingInput;
        private int inputLength;
        private VisualNode startNode;
        private int delay;

        public Executor(string input, VisualNode startNode, int delay)
        {
            this.remainingInput = input;
            inputLength = input.Length;
            this.startNode = startNode;
            this.delay = delay;
        }

        public void doWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while (!worker.CancellationPending && remainingInput.Length > 0)
            {

                if (remainingInput.Length > 1)
                {
                    remainingInput = remainingInput.Remove(1);
                }
                else
                {
                    remainingInput = "";
                }

                int remaining = inputLength - remainingInput.Length;
                Debug.WriteLine("*** remaining: " + remaining);
                worker.ReportProgress(remaining);

                System.Threading.Thread.Sleep(delay);
            }

            e.Cancel = true;
        }
    }
}
