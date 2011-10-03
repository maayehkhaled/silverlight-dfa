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
    public class VisualNode : IComparable<VisualNode>
	{

        /* default size (width/height) used for nodes grid/ellipse */
		//public static readonly double DEFAULT_SIZE = 25.0;

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
		protected string labelText;

		/* the grid on which the ellipse and label (textbox) are drawed */
		protected Grid grid;
		/* the ellipse representing the node */
		protected Ellipse ellipse;
		/* the node's label */
		protected TextBlock label;

        /* constructs a new node object */
        public VisualNode(string labelText, EPoint location, bool isStartNode, bool isEndNode)
        {
            this.location = location;
            this.isStartNode = isStartNode;
            this.isEndNode = isEndNode;
            adjacenceList = new List<Tuple<VisualNode, string>>();

			this.labelText = labelText;
        }

        /* adds a new entry to the adjacenceList of this node */
        public void addAdjacenceList(VisualNode dstNode, string label)
        /* if a node can reach another node with an edge label, add it in adjacent list
         * (with the edge label) */
        {
            var isAdded = false;
            foreach (Tuple<VisualNode, string> a in adjacenceList)
            {
                string l = a.Item1.getLabelText();
                if (l.Equals(dstNode.getLabelText()))// same node is already in list
                {
                    /* node is allready in the adjacent list, so check if the label is the same
                     or newer */
                    if (!a.Item2.Trim().Equals(label))// same node with new label for edge
                    {
                        var newEdgeLabel = a.Item2.Trim() + "|" + label.Trim();
                        Tuple<VisualNode, string> newAdjcent = 
							new Tuple<VisualNode, string>(a.Item1, newEdgeLabel);
                        adjacenceList.Add(newAdjcent);// add the new adjacent Tupel with the new label
                        adjacenceList.Remove(a);      // and remove the old one. 
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
                Tuple<VisualNode, string> t = new Tuple<VisualNode, string>(dstNode, label);
                adjacenceList.Add(t);
            }
        }

		/* adds a new entry to the map of destinations nodes of this node */
		public void addDstEdge(string symbol, VisualEdge edge)
		{
			Debug.WriteLine("***************** addDstEdge(): node: " + label.Text + ", symbol: " + symbol);
			dstEdges.Add(symbol, edge);
		}

		public VisualEdge getDstEdge(string symbol)
		{
			Debug.WriteLine("***************** getDstEdge(): node: " + label.Text + ", symbol: " + symbol);
			return dstEdges[symbol];
		}

		/* returns the grid where the nodes ellipse and label are drawn on */
		public Grid getGrid()
		{
			return grid;
		}

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


        /* get the nodes label text */
        public string getLabelText()
        {
            return labelText;
        }

		/** define equal of two VisualNode */
		public static bool operator ==(VisualNode v1, VisualNode v2)
		{
			return (v1.getLabelText() == v2.getLabelText());
		}
		public static bool operator !=(VisualNode v1, VisualNode v2)
		{
			return (v1.getLabelText() != v2.getLabelText());
		}

		override public bool  Equals(Object o)
		{
			return (this == (VisualNode)o);
		}

		override public int GetHashCode()
		{
			return this.labelText.GetHashCode();
		}

		/*define compareTo to order the state of machine */
		public int CompareTo(VisualNode v1)
		{
			return (v1.getLabelText().CompareTo(this));
		}

		public override string ToString()
		{
			return labelText;
		}

    }
}
