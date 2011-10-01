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
using Microsoft.Expression.Controls;
using System.Windows.Markup;
using System.Diagnostics;

namespace SilverlightApplication6
{
    public class VisualEdge
    {
        public static readonly Uri GRID_RESOURCE_URI = new Uri("VisualEdgeGrid.xaml", UriKind.Relative);
        private static string gridResourceString = null;

        private VisualNode dstNode;

        private Grid grid;
        private Path bezier;
        private Path arrow;
        private PathListBox animationPathListBox;
        //private PathListBox labelPathListBox;
		private TextBlock labelPathListBox;
		private string edgeLabel;

        private Storyboard animation;
        private Brush edgeBrush;

        public VisualEdge(string labelText, EPoint srcLocation, EPoint dstLocation)
        {
            grid = getVisualEdgeGrid(labelText);
            bezier = grid.FindName("bezier") as Path;
            arrow = grid.FindName("arrow") as Path;
            animationPathListBox = grid.FindName("animationPathListBox") as PathListBox;
            animationPathListBox.ItemsSource = labelText;
			edgeLabel = labelText;
			
			//labelPathListBox =  grid.FindName("labelPathListBox") as PathListBox;
            //labelPathListBox.ItemsSource = edgeLabel;

			labelPathListBox = grid.FindName("edgeLabelContainer") as TextBlock;
			labelPathListBox.Text = edgeLabel;

			animation = grid.Resources["animation"] as Storyboard;
            edgeBrush = grid.Resources["edgeBrush"] as Brush;
			

            Debug.WriteLine("*** symbol: " + labelText);
        }

        public void setDstNode(VisualNode dstNode)
        {
            this.dstNode = dstNode;
        }

        public VisualNode getDstNode()
        {
            return dstNode;
        }

        public Grid getGrid()
        {
            return grid;
        }

        public void setBezier(PathFigure pathFigure)
        {
            PathGeometry pg = new PathGeometry();
            pg.Figures.Add(pathFigure);
            bezier.Data = pg;
        }

        public void setArrow(PathGeometry arrowGeometry)
        {
            arrow.Data = arrowGeometry;
        }

        /* returns the storyboard for the source animation */
        public Storyboard getAnimation()
        {
            return animation;
        }

        /* loads a new node grid by using xamlreader */
        private static Grid getVisualEdgeGrid(string label)
        {
            if (gridResourceString == null)
            {

                System.IO.Stream s = App.GetResourceStream(GRID_RESOURCE_URI).Stream;
                System.IO.StreamReader sr = new System.IO.StreamReader(s);
                gridResourceString = sr.ReadToEnd();
            }

            Grid grid = (Grid)XamlReader.Load(gridResourceString);
            grid.Name = label + SimpleUniqueRandom.getInt();
            //grid.RenderTransform = new CompositeTransform();

            return grid;
        }

		
	}
}
