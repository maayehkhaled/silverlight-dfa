using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;

using Microsoft.Expression.Controls;

namespace SilverlightApplication6
{
    /** 
    * doc: Real programmers don't comment their code.  It was hard to write, it
      should be hard to understand and even harder to modify.
    */
	public class Node
	{
		public string nodeLabel;
		public List<Tuple<Node, string>> adjacent;

		/* x-coordinate of the topleft of the node on Silverlight Coordinate system */
		public double x;
		/* y-coordinate of the topleft of the node on Silverlight Coordinate system */
		public double y;
		public bool isEnd;
		public double width;
		public double height;

		public static double defaultSize = 25.0;

		

		public Node(string label, double x, double y, bool isEnd, double width, double height)
		{
			this.nodeLabel = label;
			this.x = x;
			this.y = y;
			this.isEnd = isEnd;
			this.adjacent = new List<Tuple<Node, string>>();

			this.width = width;
			this.height = height;
		}

		public Node(String label) : 
			this(label, 0, 0, false, defaultSize, defaultSize) { }

		public Node(String label, double x, double y, bool isEnd) :
			this(label, x, y, isEnd, defaultSize, defaultSize) { }

		public void addAdjcent(Node node, string label)
		/* if a node can reach another node with a edge label, add it in adjacent list
		 * (with the edge label) */
		{
			var isAdded = false;
			foreach (Tuple<Node, string> a in adjacent)
			{
				string l = a.Item1.nodeLabel;
				if (l.Equals(node.nodeLabel))// same node is already in list
				{
					/* node is allready in the adjacent list, so check if the label is the same
					 or newer */
					if (!a.Item2.Equals(label))// same node with new label for edge
					{
						var newEdgeLabel = a.Item2 + "|" + label;
						Tuple<Node, string> newAdjcent = new Tuple<Node, string>(a.Item1, newEdgeLabel);
						adjacent.Add(newAdjcent);
						adjacent.Remove(a);
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
				Tuple<Node, string> t = new Tuple<Node, string>(node, label);
				adjacent.Add(t);
			}
		}
	}
}
