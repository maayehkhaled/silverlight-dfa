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

namespace SilverlightApplication6
{
    public static class AnimationPlanner
    {
        private static Uri storyboardResourceDictionaryUri = new Uri("StoryboardResourceDictionary.xaml", UriKind.Relative);

        public static Storyboard createStoryboard(string input, VisualNode currentNode)
        {
            Storyboard storyboard = new Storyboard();
            
            string remainingInput = input;
            int inputLength = input.Length;

            while (remainingInput.Length > 0)
            {
                string symbol = remainingInput[0].ToString();
                Debug.WriteLine("*** processing node: " + currentNode.node.nodeLabel + ", symbol: " + symbol);

                VisualNode follower;
                if (currentNode.TryGetFollower(symbol, out follower))
                {
                    Debug.WriteLine("*** follower is: " + follower.node.nodeLabel);

                    ResourceDictionary rd = new ResourceDictionary();
                    rd.Source = storyboardResourceDictionaryUri;

                    storyboard.Duration += TimeSpan.FromSeconds(1);
                    // TODO add input falldown animation
                    addStateAnimation("srcStateAnimation", storyboard, currentNode);
                    // TODO add path animation
                    // TODO add dst animation
                    addStateAnimation("dstStateAnimation", storyboard, currentNode);

                    currentNode = follower;
                }

                if (remainingInput.Length > 1)
                {
                    remainingInput = remainingInput.Remove(0, 1);
                    int processed = inputLength - remainingInput.Length;
                    Debug.WriteLine("*** remaining: " + remainingInput.Length + " processed: " + processed);
                }
                else if (currentNode.node.isEnd)
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

            return storyboard;
        }

        private static void addSrcStateAnimation(Storyboard storyboard, VisualNode node)
        {
            EasingDoubleKeyFrame frame1 = new EasingDoubleKeyFrame();
            frame1.KeyTime = TimeSpan.FromSeconds(1);
            frame1.Value = 3;

            EasingDoubleKeyFrame frame2 = new EasingDoubleKeyFrame();
            frame2.KeyTime = TimeSpan.FromSeconds(3);
            frame2.Value = -6;

            DoubleAnimationUsingKeyFrames transX = new DoubleAnimationUsingKeyFrames();
            transX.KeyFrames.Add(frame1);
            transX.KeyFrames.Add(frame2);

            transX.set
        }
    }
}
