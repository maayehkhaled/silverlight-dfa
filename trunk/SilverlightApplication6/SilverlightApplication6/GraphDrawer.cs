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
using System.Diagnostics;

namespace SilverlightApplication6
{
	public class GraphDrawer
	{
		List<VisualNode> _visualNodes;
        public List<VisualNode> visualNodes
		{
			set { _visualNodes = value; }
			get { return _visualNodes; }
		}

		public Canvas dfaCanvas;

        public GraphDrawer(List<VisualNode> nodes, Canvas c)
		{
			//dfaCanvas = new Canvas();
			dfaCanvas = c;
            visualNodes = nodes;
		}

        public void drawDFA()
		{
            foreach (VisualNode vn in visualNodes)
			{
				drawNode(vn);
				drawOutEdges(vn);
			}
		}

        private void drawNode(VisualNode n)
		{
            Debug.WriteLine("*** drawing node: " + n.getLabelText() + ", grid parent: " + n.getGrid().Parent );
			dfaCanvas.Children.Add(n.getGrid());
            n.getGrid().SetValue(Canvas.TopProperty, n.y);
            n.getGrid().SetValue(Canvas.LeftProperty, n.x);
            n.getGrid().SetValue(Canvas.ZIndexProperty, 1);
		}

        private void drawOutEdges(VisualNode vn)
		{
            Debug.WriteLine("*** drawing outEdges of node: " + vn.getLabelText());
            foreach (Tuple<VisualNode, string> m in vn.adjacenceList)
			{
				Tuple<Path,VisualEdge> outEdge = createOutEdge(vn, m);
                outEdge.Item1.SetValue(Canvas.ZIndexProperty, -1);
                dfaCanvas.Children.Add(outEdge.Item1);

                dfaCanvas.Children.Add(outEdge.Item2.getTextBlock());
                outEdge.Item2.getTextBlock().SetValue(Canvas.TopProperty, outEdge.Item2.coordinates.y);
                outEdge.Item2.getTextBlock().SetValue(Canvas.LeftProperty, outEdge.Item2.coordinates.x);

				/* Now insert the new created outEdge in the list of outEdge of VisualNode */
				vn.outEdges.Add(outEdge);
			}
		}

        Tuple<Path, VisualEdge> createOutEdge(VisualNode node, Tuple<VisualNode, string> m)
		{
			Tuple<EPoint, EPoint, EPoint, EPoint> bezierPoints = LayoutComputer.computeEdge(node, m.Item1);
			
			Path path = createBezier(bezierPoints);
			EPoint labelCoordinate = LayoutComputer.computeEdgeLabel(bezierPoints.Item2, bezierPoints.Item3);
			VisualEdge edgeSymbol = new VisualEdge(m.Item2, labelCoordinate);
			return new Tuple<Path,VisualEdge>(path, edgeSymbol);
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
			Path path = new Path();
			path.Data = pathGeo;
			path.Stroke = (SolidColorBrush)Application.Current.Resources["edgeBrush"];
            path.StrokeThickness = 2;
			return path;
		}
	}

	
}
