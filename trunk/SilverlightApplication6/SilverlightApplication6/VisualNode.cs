using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;

using Microsoft.Expression.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Diagnostics;

/* "Real programmers don't comment their code. It was hard to write, it
 * should be hard to understand and even harder to modify."
 */
namespace SilverlightApplication6
{
    /* Represents a node (state) of the DFA */
    public class VisualNode {

        /* default size (width/height) used for nodes grid/ellipse */
        //public static readonly double DEFAULT_SIZE = 25.0;
        public static readonly Uri COMMON_GRID_RESOURCE_URI = new Uri("VisualNodeGrid.xaml", UriKind.Relative);
        public static readonly Uri STARTEND_GRID_RESOURCE_URI = new Uri("VisualStartAndAcceptedNodeGrid.xaml", UriKind.Relative);
        public static readonly Uri START_GRID_RESOURCE_URI = new Uri("VisualStartNodeGrid.xaml", UriKind.Relative);
        public static readonly Uri END_GRID_RESOURCE_URI = new Uri("VisualAcceptNodeGrid.xaml", UriKind.Relative);
        private static string gridResourceString = null;

        ///* x-coordinate of the topleft of the node on Silverlight Coordinate system */
        //public readonly double x;
        ///* y-coordinate of the topleft of the node on Silverlight Coordinate system */
        //public readonly double y;
        public readonly EPoint location;
        /* is the node a start node? */
        public readonly bool isStartNode = false;
        /* is the node an end node? */
        public readonly bool isEndNode = false;

        /* contains all nodes (states) that can be reached through this node */
        public readonly List<Tuple<VisualNode, string>> adjacenceList;
        /* allows to find the next node for the given symbol string */
        //private IDictionary<string, VisualNode> dstNodes = new Dictionary<string, VisualNode>();
        private IDictionary<string, VisualEdge> dstEdges = new Dictionary<string, VisualEdge>();

        /* the grid on which the ellipse and label (textbox) are drawed */
        private Grid grid;
        /* the ellipse representing the node */
        private Ellipse ellipse;
        /* the node's label */
        private TextBlock label;

        private Storyboard srcAnimation;
        private Storyboard dstAnimation;
        private Storyboard acceptedAnimation;
        private Storyboard rejectedAnimation;

        /* constructs a new node object */
        public VisualNode(string labelText, EPoint location, bool isStartNode, bool isEndNode)
        {
            this.location = location;
            this.isStartNode = isStartNode;
            this.isEndNode = isEndNode;
            adjacenceList = new List<Tuple<VisualNode, string>>();

            // load grid as clone
            if (isStartNode && isEndNode)
            {
                Debug.WriteLine("*** is start and end node...");
                grid = getVisualNodeGrid(labelText, STARTEND_GRID_RESOURCE_URI);
            }
            else if (isStartNode)
            {
                Debug.WriteLine("*** is only start node...");
                grid = getVisualNodeGrid(labelText, START_GRID_RESOURCE_URI);
            }
            else if (isEndNode)
            {
                Debug.WriteLine("*** is only end node...");
                grid = getVisualNodeGrid(labelText, END_GRID_RESOURCE_URI);
            }
            else
            {
                Debug.WriteLine("*** is a common node...");
                grid = getVisualNodeGrid(labelText, COMMON_GRID_RESOURCE_URI);
            }

            // initialize animations
            srcAnimation = grid.Resources["srcAnimation"] as Storyboard;
            //Storyboard.SetTarget(srcAnimation, grid);
            Storyboard.SetTargetName(srcAnimation, labelText);
            //Debug.WriteLine("*** source animation children: " + srcAnimation.Children.Count);

            dstAnimation = grid.Resources["dstAnimation"] as Storyboard;
            //Storyboard.SetTarget(dstAnimation, grid);
            Storyboard.SetTargetName(dstAnimation, labelText);
            //Debug.WriteLine("*** destination animation children: " + dstAnimation.Children.Count);

            acceptedAnimation = grid.Resources["acceptedAnimation"] as Storyboard;
            rejectedAnimation = grid.Resources["rejectedAnimation"] as Storyboard;

            ellipse = grid.FindName("ellipse") as Ellipse;

            label = grid.FindName("label") as TextBlock;
            label.Text = labelText;

            //Debug.WriteLine("*** ellipse name: " + ellipse.Name);

            //grid.Width = DEFAULT_SIZE;
            //grid.Height = DEFAULT_SIZE;
            //ellipse.Width = DEFAULT_SIZE;
            //ellipse.Height = DEFAULT_SIZE;
        }

        /* adds a new entry to the adjacenceList of this node */
        public void addAdjacenceList(VisualNode node, string label)
        /* if a node can reach another node with an edge label, add it in adjacent list
         * (with the edge label) */
        {
            var isAdded = false;
            foreach (Tuple<VisualNode, string> a in adjacenceList)
            {
                string l = a.Item1.getLabelText();
                if (l.Equals(node.getLabelText()))// same node is already in list
                {
                    /* node is allready in the adjacent list, so check if the label is the same
                     or newer */
                    if (!a.Item2.Equals(label))// same node with new label for edge
                    {
                        var newEdgeLabel = a.Item2 + "|" + label;
                        Tuple<VisualNode, string> newAdjcent = new Tuple<VisualNode, string>(a.Item1, newEdgeLabel);
                        adjacenceList.Add(newAdjcent);
                        adjacenceList.Remove(a);
                        isAdded = true;
                        break;
                    }
                    else // same node and same edge label; nothing to do
                    {
                        isAdded = true;
                        break;
                    }

                }
            }
            if (!isAdded)
            {
                Tuple<VisualNode, string> t = new Tuple<VisualNode, string>(node, label);
                adjacenceList.Add(t);
            }
        }

        /* adds a new entry to the map of destinations nodes of this node */
        //public void addDstNode(string symbol, VisualNode node)
        //{
        //    dstNodes.Add(symbol, node);
        //}

        /* retrieves, if possible, the node that can be reached through the given symbol. */
        public bool TryGetDstNode(string symbol, out VisualNode node)
        {
            VisualEdge ve;
            if (dstEdges.TryGetValue(symbol, out ve))
            {
                node = ve.getDstNode();
                return true;
            }
            else
            {
                node = null;
                return false;
            }
        }

        /* adds a new entry to the map of destinations nodes of this node */
        public void addDstEdge(string symbol, VisualEdge edge)
        {
            dstEdges.Add(symbol, edge);
        }

        public VisualEdge getDstEdge(string symbol)
        {
            return dstEdges[symbol];
        }

        /* returns the grid where the nodes ellipse and label are drawn on */
        public Grid getGrid()
        {
            return grid;
        }

        /* returns the nodes ellipse object */
        public Ellipse getEllipse()
        {
            return ellipse;
        }

        /* returns the textblock used for the nodes label */
        public TextBlock getTextBlock()
        {
            return label;
        }

        /* get the nodes label text */
        public string getLabelText()
        {
            return label.Text;
        }

        /* returns the storyboard for the source animation */
        public Storyboard getSrcAnimation()
        {
            return srcAnimation;
        }

        /* returns the storyboard for the destination animation */
        public Storyboard getDstAnimation()
        {
            return dstAnimation;
        }

        /* returns the storyboard for the accepted animation */
        public Storyboard getAcceptedAnimation()
        {
            return acceptedAnimation;
        }

        /* returns the storyboard for the rejected animation */
        public Storyboard getRejectedAnimation()
        {
            return rejectedAnimation;
        }

        /* loads a new node grid by using xamlreader */
        private static Grid getVisualNodeGrid(string label, Uri type)
        {
            // TODO re-enable gridResourceString
            //if (gridResourceString == null)
            //{
                System.IO.Stream s = App.GetResourceStream(type).Stream;
                System.IO.StreamReader sr = new System.IO.StreamReader(s);
                gridResourceString = sr.ReadToEnd();
            //}
                //Debug.WriteLine(gridResourceString);
            Grid grid = (Grid) XamlReader.Load(gridResourceString);
            grid.Name = label;
            //grid.RenderTransform = new CompositeTransform();

            return grid;
        }
    }
}
