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
        public static Storyboard createStoryboard(string input, VisualNode currentNode)
        {
            Storyboard storyboard = new Storyboard();
            double startSecond = 0;

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

                    // TODO add input falldown animation
                    startSecond = addSrcStateAnimation(storyboard, currentNode, startSecond);
                    // TODO add path animation
                    // TODO add dst animation
                    startSecond = addDstStateAnimation(storyboard, follower, startSecond);

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

        private static EasingDoubleKeyFrame newEasingDoubleKeyFrame(double seconds, double value) {
            EasingDoubleKeyFrame frame = new EasingDoubleKeyFrame();
            frame.KeyTime = TimeSpan.FromSeconds(seconds);
            frame.Value = value;

            return frame;
        }

        private static double addSrcStateAnimation(Storyboard storyboard, VisualNode node, double startSecond)
        {
            int transConst = 4;
            int scaleConst = 8;

            // translate grid x
            DoubleAnimationUsingKeyFrames transX = new DoubleAnimationUsingKeyFrames();
            transX.BeginTime = TimeSpan.FromSeconds(startSecond);
            transX.KeyFrames.Add(newEasingDoubleKeyFrame(1, node.node.x - transConst));
            transX.KeyFrames.Add(newEasingDoubleKeyFrame(2, node.node.x + transConst));
            transX.KeyFrames.Add(newEasingDoubleKeyFrame(3, node.node.x - transConst));
            transX.KeyFrames.Add(newEasingDoubleKeyFrame(4, node.node.x + transConst));
            transX.KeyFrames.Add(newEasingDoubleKeyFrame(5, node.node.x - transConst));
            transX.KeyFrames.Add(newEasingDoubleKeyFrame(6, node.node.x));
            transX.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTarget(transX, node.state);
            storyboard.Children.Add(transX);

            // translate grid y
            DoubleAnimationUsingKeyFrames transY = new DoubleAnimationUsingKeyFrames();
            transY.BeginTime = TimeSpan.FromSeconds(startSecond);
            transY.KeyFrames.Add(newEasingDoubleKeyFrame(1, node.node.y - transConst));
            transY.KeyFrames.Add(newEasingDoubleKeyFrame(2, node.node.y + transConst));
            transY.KeyFrames.Add(newEasingDoubleKeyFrame(3, node.node.y - transConst));
            transY.KeyFrames.Add(newEasingDoubleKeyFrame(4, node.node.y + transConst));
            transY.KeyFrames.Add(newEasingDoubleKeyFrame(5, node.node.y - transConst));
            transY.KeyFrames.Add(newEasingDoubleKeyFrame(6, node.node.y));
            transY.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("(Canvas.Top)"));
            Storyboard.SetTarget(transY, node.state);
            storyboard.Children.Add(transY);

            // scale grid x
            DoubleAnimationUsingKeyFrames scaleX = new DoubleAnimationUsingKeyFrames();
            scaleX.BeginTime = TimeSpan.FromSeconds(startSecond);
            scaleX.KeyFrames.Add(newEasingDoubleKeyFrame(1, node.node.width + scaleConst));
            scaleX.KeyFrames.Add(newEasingDoubleKeyFrame(2, node.node.width - scaleConst));
            scaleX.KeyFrames.Add(newEasingDoubleKeyFrame(3, node.node.width + scaleConst));
            scaleX.KeyFrames.Add(newEasingDoubleKeyFrame(4, node.node.width - scaleConst));
            scaleX.KeyFrames.Add(newEasingDoubleKeyFrame(5, node.node.width + scaleConst));
            scaleX.KeyFrames.Add(newEasingDoubleKeyFrame(6, node.node.width));
            scaleX.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("(Grid.Width)"));
            Storyboard.SetTarget(scaleX, node.state);
            storyboard.Children.Add(scaleX);

            // scale ellipse x
            DoubleAnimationUsingKeyFrames scaleX2 = new DoubleAnimationUsingKeyFrames();
            scaleX2.BeginTime = TimeSpan.FromSeconds(startSecond);
            scaleX2.KeyFrames.Add(newEasingDoubleKeyFrame(1, node.node.width + scaleConst));
            scaleX2.KeyFrames.Add(newEasingDoubleKeyFrame(2, node.node.width - scaleConst));
            scaleX2.KeyFrames.Add(newEasingDoubleKeyFrame(3, node.node.width + scaleConst));
            scaleX2.KeyFrames.Add(newEasingDoubleKeyFrame(4, node.node.width - scaleConst));
            scaleX2.KeyFrames.Add(newEasingDoubleKeyFrame(5, node.node.width + scaleConst));
            scaleX2.KeyFrames.Add(newEasingDoubleKeyFrame(6, node.node.width));
            scaleX2.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("(Ellipse.Width)"));
            Storyboard.SetTarget(scaleX2, node.e);
            storyboard.Children.Add(scaleX2);

            // scale grid y
            DoubleAnimationUsingKeyFrames scaleY = new DoubleAnimationUsingKeyFrames();
            scaleY.BeginTime = TimeSpan.FromSeconds(startSecond);
            scaleY.KeyFrames.Add(newEasingDoubleKeyFrame(1, node.node.height + scaleConst));
            scaleY.KeyFrames.Add(newEasingDoubleKeyFrame(2, node.node.height - scaleConst));
            scaleY.KeyFrames.Add(newEasingDoubleKeyFrame(3, node.node.height + scaleConst));
            scaleY.KeyFrames.Add(newEasingDoubleKeyFrame(4, node.node.height - scaleConst));
            scaleY.KeyFrames.Add(newEasingDoubleKeyFrame(5, node.node.height + scaleConst));
            scaleY.KeyFrames.Add(newEasingDoubleKeyFrame(6, node.node.height));
            scaleY.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("(Grid.Height)"));
            Storyboard.SetTarget(scaleY, node.state);
            storyboard.Children.Add(scaleY);

            // scale ellipse y
            DoubleAnimationUsingKeyFrames scaleY2 = new DoubleAnimationUsingKeyFrames();
            scaleY2.BeginTime = TimeSpan.FromSeconds(startSecond);
            scaleY2.KeyFrames.Add(newEasingDoubleKeyFrame(1, node.node.height + scaleConst));
            scaleY2.KeyFrames.Add(newEasingDoubleKeyFrame(2, node.node.height - scaleConst));
            scaleY2.KeyFrames.Add(newEasingDoubleKeyFrame(3, node.node.height + scaleConst));
            scaleY2.KeyFrames.Add(newEasingDoubleKeyFrame(4, node.node.height - scaleConst));
            scaleY2.KeyFrames.Add(newEasingDoubleKeyFrame(5, node.node.height + scaleConst));
            scaleY2.KeyFrames.Add(newEasingDoubleKeyFrame(6, node.node.height));
            scaleY2.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("(Ellipse.Height)"));
            Storyboard.SetTarget(scaleY2, node.e);
            storyboard.Children.Add(scaleY2);

            return startSecond + 6;
        }

        private static double addDstStateAnimation(Storyboard storyboard, VisualNode node, double startSecond)
        {
            int transConst = 4;

            // translate x
            DoubleAnimationUsingKeyFrames transX = (DoubleAnimationUsingKeyFrames)storyboard.Children[0];
            transX.KeyFrames.Add(newEasingDoubleKeyFrame(startSecond + 1, node.node.x - transConst));
            transX.KeyFrames.Add(newEasingDoubleKeyFrame(startSecond + 2, node.node.x + transConst));
            transX.KeyFrames.Add(newEasingDoubleKeyFrame(startSecond + 3, node.node.x - transConst));
            transX.KeyFrames.Add(newEasingDoubleKeyFrame(startSecond + 4, node.node.x + transConst));
            transX.KeyFrames.Add(newEasingDoubleKeyFrame(startSecond + 5, node.node.x - transConst));
            transX.KeyFrames.Add(newEasingDoubleKeyFrame(startSecond + 6, node.node.x));

            // translate y
            DoubleAnimationUsingKeyFrames transY = (DoubleAnimationUsingKeyFrames)storyboard.Children[1];
            transY.KeyFrames.Add(newEasingDoubleKeyFrame(startSecond + 1, node.node.y - transConst));
            transY.KeyFrames.Add(newEasingDoubleKeyFrame(startSecond + 2, node.node.y + transConst));
            transY.KeyFrames.Add(newEasingDoubleKeyFrame(startSecond + 3, node.node.y - transConst));
            transY.KeyFrames.Add(newEasingDoubleKeyFrame(startSecond + 4, node.node.y + transConst));
            transY.KeyFrames.Add(newEasingDoubleKeyFrame(startSecond + 5, node.node.y - transConst));
            transY.KeyFrames.Add(newEasingDoubleKeyFrame(startSecond + 6, node.node.y));

            return startSecond + 6;
        }
    }
}
