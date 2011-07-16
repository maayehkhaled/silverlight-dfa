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
				Tuple<Path,VisualSymbol> outEdge = createOutEdge(vn.node, m);
				dfaCanvas.Children.Add(outEdge.Item1);

				dfaCanvas.Children.Add(outEdge.Item2.label );
				outEdge.Item2.label.SetValue(Canvas.TopProperty, outEdge.Item2.coordinate.y);
				outEdge.Item2.label.SetValue(Canvas.LeftProperty, outEdge.Item2.coordinate.x);

				/* Now insert the new created outEdge in the list of outEdge of VisualNote */
				vn.outEdges.Add(outEdge);
			}
		}

		Tuple<Path,VisualSymbol> createOutEdge(Node node, Tuple<Node, string> m)
		{
			Tuple<EPoint, EPoint, EPoint, EPoint> bezierPoints = LayoutComputer.computeEdge(node, m.Item1);
			
			Path path = createBezier(bezierPoints);
			EPoint labelCoordinate = LayoutComputer.computeEdgeLabel(bezierPoints.Item2, bezierPoints.Item3);
			VisualSymbol edgeSymbol = new VisualSymbol(m.Item2, labelCoordinate);
			return new Tuple<Path,VisualSymbol>(path, edgeSymbol);
		}


		Path createBezier(Tuple<EPoint,EPoint,EPoint,EPoint> bezierPoints)
		{
			PathFigure bezierFigure = new PathFigure();
			bezierFigure.StartPoint = new Point(
				bezierPoints.Item1.x, 
				bezierPoints.Item1.y);
			BezierSegment bz = new BezierSegment();
			bz.Point1 = new Point(bezierPoints.Item2.x, bezierPoints.Item2.y);
			bz.Point2 = new Point(bezierPoints.Item3.x, bezierPoints.Item3.y);
			bz.Point3 = new Point(bezierPoints.Item4.x, bezierPoints.Item4.y);
			bezierFigure.Segments.Add(bz);

			Tuple<EPoint, EPoint> arrowInfo = LayoutComputer.computeArrow(bezierPoints.Item4, bezierPoints.Item3);
			PathFigure arrowFigure = new PathFigure();
			arrowFigure.StartPoint = new Point(
				arrowInfo.Item1.x,
				arrowInfo.Item1.y
				);
			
			LineSegment wing1 = new LineSegment();
			wing1.Point = new Point(
				bezierPoints.Item4.x,
				bezierPoints.Item4.y
				);
			arrowFigure.Segments.Add(wing1);

			LineSegment wing2 = new LineSegment();
			wing2.Point = new Point(
				arrowInfo.Item2.x,
				arrowInfo.Item2.y
				);
			arrowFigure.Segments.Add(wing2);

			PathGeometry pathGeo = new PathGeometry();
			pathGeo.Figures.Add(bezierFigure);  //= pathFigCollection;
			pathGeo.Figures.Add(arrowFigure);

			/* TODO: use a static class to choose color (see Visual.cs) */
			SolidColorBrush edgeColor = new SolidColorBrush(Colors.Brown);
			Path path = new Path();
			path.Data = pathGeo;
			path.Stroke = edgeColor;
			return path;
		}
	}

	
}
