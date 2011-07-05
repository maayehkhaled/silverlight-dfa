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
using System.Collections.Generic;

namespace SilverlightApplication6
{
	public class GraphDrawer
	{
		List<Node> nodes;
		List<VisualNode> visualNodes;
		public Canvas dfaCanvas;

		public GraphDrawer(List<Node> nodes, Canvas c)
		{
			this.nodes = nodes;
			//dfaCanvas = new Canvas();
			dfaCanvas = c;
			visualNodes = new List<VisualNode>();
			foreach (Node n in nodes)
			{
				visualNodes.Add(new VisualNode(n));
			}

		}

		public void drawDFA()
		{
			foreach (VisualNode vn in visualNodes)
			{
				drawNode(vn);
				drawOutEdge(vn);
			}
		}

		private void drawNode(VisualNode n)
		{
			dfaCanvas.Children.Add(n.state);
			n.state.SetValue(Canvas.TopProperty, n.node.y);
			n.state.SetValue(Canvas.LeftProperty, n.node.x);
		}

		private void drawOutEdge(VisualNode vn)
		{
			foreach (Tuple<Node,string> m in vn.node.adjacent)
			{
				Line edge = createOutEdge(vn.node, m);

				dfaCanvas.Children.Add(edge);

			}
		}

		Line createOutEdge(Node node, Tuple<Node, string> m)
		{
			Tuple<EPoint, EPoint, EPoint, EPoint> bezierPoints = LayoutComputer.computeEdge(node, m.Item1);
			Line path = new Line();
			
			path.X1 = bezierPoints.Item1.x;
			path.Y1 = bezierPoints.Item1.y;

			path.X2 = bezierPoints.Item4.x;
			path.Y2 = bezierPoints.Item4.y;
			
			/*
			path.X1 = 0;
			path.Y1 = 0;
			path.X2 = 200;
			path.Y2 = 200;
			*/

			SolidColorBrush edgeColor = new SolidColorBrush(Colors.Brown);
			path.Fill = edgeColor;
			path.StrokeThickness = 3;
			path.Stroke = edgeColor;

			return path;
		}

	}
}
