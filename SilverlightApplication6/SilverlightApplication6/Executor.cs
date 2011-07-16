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

namespace SilverlightApplication6
{
    public class Executor
    {
        private string remainingInput;
        private int inputLength;
        private VisualNode startNode;

        public Executor(string input, VisualNode startNode)
        {
            this.remainingInput = input;
            inputLength = input.Length;
            this.startNode = startNode;
        }

        public void doWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if ((worker.CancellationPending == true))
            {
                e.Cancel = true;
                return;
            }
            else
            {
                remainingInput.Remove(1);
                worker.ReportProgress(remainingInput.Length - inputLength);
            }
        }
    }
}
