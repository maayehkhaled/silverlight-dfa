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
using System.Windows.Threading;

namespace SilverlightApplication6
{
    public class Executor
    {
        private VisualNode currentNode;
        private int delay;

        private string remainingInput;
        private int inputLength;

        public void doWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while (!worker.CancellationPending && remainingInput.Length > 0)
            {
                string symbol = remainingInput[0].ToString();
                Debug.WriteLine("*** processing node: " + currentNode.node.nodeLabel + ", symbol: " + symbol);

                VisualNode follower;
                if (currentNode.TryGetFollower(symbol, out follower))
                {
                    Debug.WriteLine("*** follower is: " + follower.node.nodeLabel);

                    worker.ReportProgress(0, currentNode);
                    worker.ReportProgress(0, follower);
                    currentNode = follower;
                }

                if (remainingInput.Length > 1)
                {
                    remainingInput = remainingInput.Remove(0, 1);
                    int processed = inputLength - remainingInput.Length;
                    Debug.WriteLine("*** remaining: " + remainingInput.Length + " processed: " + processed);
                    worker.ReportProgress(processed);

                    System.Threading.Thread.Sleep(delay);
                }
                else if (currentNode.node.isEnd)
                {
                    Debug.WriteLine("*** accepting...");
                    e.Result = "Accepted :-)";
                    return;
                }
                else
                {
                    Debug.WriteLine("*** rejecting...");
                    e.Result = "Rejected :-/";
                    return;
                }
            }

            e.Cancel = true;
        }

        public void setStartNode(VisualNode startNode)
        {
            this.currentNode = startNode;
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

        private delegate void InvokeThrowSymbolDelegate(VisualNode node);

        private void InvokeThrowSymbol(VisualNode node)
        {
            node.throwSymbol();
        }
    }
}
