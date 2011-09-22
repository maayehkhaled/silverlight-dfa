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
using Microsoft.Expression.Controls;

namespace SilverlightApplication6
{
    // used to draw the graph
	public class GraphDrawer
	{
        private List<VisualNode> visualNodes;

		public Canvas dfaCanvas;

        public GraphDrawer(List<VisualNode> nodes, Canvas c)
		{
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
            Debug.WriteLine("*** drawing node: " + n.getLabelText());
			dfaCanvas.Children.Add(n.getGrid());
            n.getGrid().SetValue(Canvas.TopProperty, n.location.y);
            n.getGrid().SetValue(Canvas.LeftProperty, n.location.x);
            n.getGrid().SetValue(Canvas.ZIndexProperty, 1);
		}

        private void drawOutEdges(VisualNode vn)
		{
            Debug.WriteLine("*** drawing outEdges of node: " + vn.getLabelText());
            foreach (Tuple<VisualNode, string> t in vn.adjacenceList)
			{
                Tuple<EPoint, EPoint, EPoint, EPoint> bezierPoints = LayoutComputer.computeEdge(vn, t.Item1);
                VisualEdge visualEdge = createOutEdgeWithArrow(t.Item2, bezierPoints);
                visualEdge.setDstNode(t.Item1);

                visualEdge.getGrid().SetValue(Canvas.ZIndexProperty, -2);
                //visualEdge.getArrow().SetValue(Canvas.ZIndexProperty, -2);
                //visualEdge.getTextBlock().SetValue(Canvas.ZIndexProperty, -1);

                dfaCanvas.Children.Add(visualEdge.getGrid());
                //dfaCanvas.Children.Add(visualEdge.getTextBlock());
                //dfaCanvas.Children.Add(visualEdge.getArrow());
                //dfaCanvas.Children.Add(visualEdge.getPathListBox());

				/* Now insert the new created outEdge in the list of outEdge of VisualNode */
                if (!t.Item2.Contains("|"))
                {
                    vn.addDstEdge(t.Item2, visualEdge);
                }
                else
                {
                    string[] symbolS = t.Item2.Split('|');
                    for (int i = 0; i < symbolS.Length; i++)
                    {
                        vn.addDstEdge(symbolS[i], visualEdge);
                    }
                }
                
			}
		}

        private PathFigure createBezier(Tuple<EPoint, EPoint, EPoint, EPoint> bezierPoints)
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

            return bezierFigure;
        }

        private VisualEdge createOutEdgeWithArrow(string label, Tuple<EPoint, EPoint, EPoint, EPoint> bezierPoints)
		{
            VisualEdge visualEdge = new VisualEdge(label, bezierPoints.Item1, bezierPoints.Item4);
            visualEdge.setBezier(createBezier(bezierPoints));

            PathFigure bezierFigure = createBezier(bezierPoints);

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

            visualEdge.setArrow(pathGeo);
			                      

            return visualEdge;
		}
	}
}
