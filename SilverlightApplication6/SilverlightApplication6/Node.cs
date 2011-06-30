using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;

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
		public double x;
		public double y;
		public bool isEnd;

		public Grid presentor;

		public Node(string label, double x, double y, bool isEnd)
		{
			this.nodeLabel = label;
			this.x = x;
			this.y = y;
			this.isEnd = isEnd;
			this.adjacent = new List<Tuple<Node, String>>();
			
			presentor = new Grid();
			Ellipse e = new Ellipse();
			e.Width = 50.0;
			e.Height = 50.0;
			e.Fill = new SolidColorBrush(Colors.Gray);

			var lb = new TextBlock();
			lb.Text = label;
			lb.VerticalAlignment = VerticalAlignment.Center;
			lb.HorizontalAlignment = HorizontalAlignment.Center;
			
			presentor.Children.Add(e);
			presentor.Children.Add(lb);
		}

		public Node(String label) : this(label, 0, 0, false) { }

		public void addAdjcent(Node node, string label)
		{
			Tuple<Node, string> t = new Tuple<Node, string>(node, label);
			this.adjacent.Add(t);
		}
	}
}
