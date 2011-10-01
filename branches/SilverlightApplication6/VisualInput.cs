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
using System.Windows.Markup;
using System.Diagnostics;

namespace SilverlightApplication6
{
    public class VisualInput
    {

        public static readonly Uri GRID_RESOURCE_URI = new Uri("VisualInputGrid.xaml", UriKind.Relative);
        private static string gridResourceString = null;

        private EPoint location;

        private Grid grid;
        private TextBlock label;
        private Storyboard consumeAnimation;
        private Storyboard fadeInAnimation;
        private Storyboard searchAnimation;

        public VisualInput()
        {
            // load grid as clone
            grid = getVisualNodeGrid();
            label = grid.FindName("label") as TextBlock;

            // pre-initialize animations
            consumeAnimation = grid.Resources["consumeAnimation"] as Storyboard;
            Storyboard.SetTargetName(consumeAnimation, grid.Name);
            fadeInAnimation = grid.Resources["fadeInAnimation"] as Storyboard;
            Storyboard.SetTargetName(fadeInAnimation, grid.Name);
            searchAnimation = grid.Resources["searchAnimation"] as Storyboard;
            Storyboard.SetTargetName(searchAnimation, grid.Name);
        }

        public void setLocation(EPoint location)
        {
            Debug.WriteLine("*** setting location: location.x: " + location.x + ", location.y: " + location.y);
            this.location = location;
            grid.SetValue(Canvas.LeftProperty, location.x);
            grid.SetValue(Canvas.TopProperty, location.y);

            (consumeAnimation.Children[0] as DoubleAnimation).From = location.x;
            (consumeAnimation.Children[1] as DoubleAnimation).From = location.y;

            //DoubleAnimation movementX = grid.FindName("movementX") as DoubleAnimation;
            //movementX.From = location.x;
            //DoubleAnimation movementY = grid.FindName("movementY") as DoubleAnimation;
            //movementY.From = location.y;
        }

        public Grid getGrid()
        {
            return grid;
        }

        public TextBlock getTextBlock()
        {
            return label;
        }

        public void setLabelText(string labelText)
        {
            this.label.Text = labelText;
        }

        public void setConsumeAnimationTo(EPoint to)
        {
            Debug.WriteLine("*** setting consume animation: to.x: " + to.x + ", to.y: " + to.y);
            (consumeAnimation.Children[0] as DoubleAnimation).To = to.x;
            (consumeAnimation.Children[1] as DoubleAnimation).To = to.y;

            //DoubleAnimation movementX = grid.FindName("movementX") as DoubleAnimation;
            //movementX.To = to.x;
            //DoubleAnimation movementY = grid.FindName("movementY") as DoubleAnimation;
            //movementY.To = to.y;



    //        Debug.WriteLine("*** " + (consumeAnimation.Children[0] as DoubleAnimation).From
    //            + " " + (consumeAnimation.Children[0] as DoubleAnimation).To);
    //        Debug.WriteLine("*** " + (consumeAnimation.Children[1] as DoubleAnimation).From
    //+ " " + (consumeAnimation.Children[1] as DoubleAnimation).To);
        }

        public Storyboard getFadeInAnimation()
        {
            return fadeInAnimation;
        }

        public Storyboard getConsumeAnimation()
        {
            return consumeAnimation;
        }

        public Storyboard getSearchAnimation()
        {
            return searchAnimation;
        }

        /* loads a new node grid by using xamlreader */
        private static Grid getVisualNodeGrid()
        {
            if (gridResourceString == null)
            {
                System.IO.Stream s = App.GetResourceStream(GRID_RESOURCE_URI).Stream;
                System.IO.StreamReader sr = new System.IO.StreamReader(s);
                gridResourceString = sr.ReadToEnd();
            }

            Grid grid = (Grid)XamlReader.Load(gridResourceString);
            grid.Name = "visualInput" + SimpleUniqueRandom.getInt().ToString();

            return grid;
        }
    }
}
