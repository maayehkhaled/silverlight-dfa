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
    // static class for planning the animation sequence
    public static class AnimationPlanner
    {
        // creates an animation sequence
        public static List<Storyboard> createPlan(string input, VisualNode currentNode, List<VisualInput> visualInput, bool flowingInput)
        {
            List<Storyboard> animations = new List<Storyboard>();

            string remainingInput = input;
            int inputLength = input.Length;
            int processed = 0;

            if (flowingInput)
            {
                for (int i = 0; i < inputLength; i++)
                {
                    Debug.WriteLine("*** adding fadein: " + i);
                    animations.Add(visualInput[i].getFadeInAnimation());
                }
            }

            while (remainingInput.Length > 0)
            {
                string symbol = remainingInput[0].ToString();
                Debug.WriteLine("*** processing node: " + currentNode.getLabelText() + ", symbol: " + symbol);

                VisualNode follower;
                if (currentNode.TryGetDstNode(symbol, out follower))
                {
                    Debug.WriteLine("*** follower is: " + follower.getLabelText());

                    // quick'n'dirty falldown animations
                    if (flowingInput)
                    {
                        VisualInput vi = visualInput[processed];
                        vi.setConsumeAnimationTo(currentNode.location);
                        vi.setLabelText(symbol);
                        animations.Add(vi.getConsumeAnimation());
                        animations.Add(vi.getSearchAnimation());
                    }
                    
                    animations.Add(currentNode.getSrcAnimation());

                    animations.Add(currentNode.getDstEdge(symbol).getAnimation());
                    
                    animations.Add(follower.getDstAnimation());

                    currentNode = follower;
                }

                if (remainingInput.Length > 1)
                {
                    remainingInput = remainingInput.Remove(0, 1);
                    processed = inputLength - remainingInput.Length;
                    Debug.WriteLine("*** remaining: " + remainingInput.Length + " processed: " + processed);
                }
                else if (currentNode.isEndNode)
                {
                    Debug.WriteLine("*** will accept...");
                    animations.Add(currentNode.getAcceptedAnimation());
                    break;
                }
                else
                {
                    Debug.WriteLine("*** will reject");
                    animations.Add(currentNode.getRejectedAnimation());
                    break;
                }
            }

            return animations;
        }
    }
}
