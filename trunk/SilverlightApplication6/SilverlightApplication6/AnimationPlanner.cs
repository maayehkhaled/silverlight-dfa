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
using System.Diagnostics;
using System.Collections.Generic;

namespace SilverlightApplication6
{
    public static class AnimationPlanner
    {
        public static Queue<Storyboard> execute(string input, VisualNode currentNode)
        {
            Queue<Storyboard> animations = new Queue<Storyboard>();

            string remainingInput = input;
            int inputLength = input.Length;

            while (remainingInput.Length > 0)
            {
                string symbol = remainingInput[0].ToString();
                Debug.WriteLine("*** processing node: " + currentNode.getLabelText() + ", symbol: " + symbol);

                VisualNode follower;
                if (currentNode.TryGetDstNode(symbol, out follower))
                {
                    Debug.WriteLine("*** follower is: " + follower.getLabelText());

                    // TODO add input falldown animation
                    animations.Enqueue(currentNode.getSrcAnimation());
                    // TODO add path animation
                    animations.Enqueue(follower.getDstAnimation());

                    currentNode = follower;
                }

                if (remainingInput.Length > 1)
                {
                    remainingInput = remainingInput.Remove(0, 1);
                    int processed = inputLength - remainingInput.Length;
                    Debug.WriteLine("*** remaining: " + remainingInput.Length + " processed: " + processed);
                }
                else if (currentNode.isEndNode)
                {
                    Debug.WriteLine("*** will accept...");
                    // TODO add all good animation
                    break;
                }
                else
                {
                    Debug.WriteLine("*** will reject");
                    // TODO add explosion animation
                    break;
                }
            }

            return animations;
        }
    }
}
