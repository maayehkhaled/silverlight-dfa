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
		public List<Grid> outEdges;

		public double x;
		public double y;
		public bool isEnd;
		
		public static double defaultSize = 25.0;

		public Grid presentor;

		public Node(string label, double x, double y, bool isEnd, double width, double height)
		{
			this.nodeLabel = label;
			this.x = x;
			this.y = y;
			this.isEnd = isEnd;
			this.adjacent = new List<Tuple<Node, String>>();
			this.outEdges = new List<Grid>();

			presentor = new Grid();
			Ellipse e = new Ellipse();
			e.Width = width;
			e.Height = height;
			e.Fill = new SolidColorBrush(Colors.LightGray);

			/* set border of ellipse */
			SolidColorBrush border = new SolidColorBrush();
			
			if (isEnd)
			{
				/* if the node is the end state, its border is  */
				border.Color = Color.FromArgb(255, 139, 0, 0);
			}else
			{
				border.Color = Colors.Black;
				
			}
			e.StrokeThickness = width/20.0;
			e.Stroke = border;


			var lb = new TextBlock();
			lb.Text = label;
			lb.VerticalAlignment = VerticalAlignment.Center;
			lb.HorizontalAlignment = HorizontalAlignment.Center;
			
			presentor.Children.Add(e);
			presentor.Children.Add(lb);
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
				if ( l.Equals(node.nodeLabel) )// same node is already in list
				{
					/*node is allready in the adjacent list, so check if the label is the same
					 or newer */
					if (!a.Item2.Equals(label))// new label for edge
					{
						var newEdgeLabel = a.Item2 + "|" + label;
						Tuple<Node, string> newAdjcent = new Tuple<Node, string>(a.Item1, newEdgeLabel);
						adjacent.Add(newAdjcent);
						adjacent.Remove(a);
						isAdded = true;
						break;
					}else // same node and same edge label; nothing to do
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

			/* create a LineArrow and a TextBlock for the outedge */
			LineArrow line = new LineArrow();
			line.Width = Math.Abs(x - node.x);
			line.Height = Math.Abs(y - node.y);
		}

		public void justifyPostion()
		{
			presentor.SetValue(Canvas.TopProperty, y);
			presentor.SetValue(Canvas.LeftProperty, x);
		}

		public void justifyOutEdges()
		{
			/*TODO*/
		}
	}
}
