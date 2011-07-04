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

using Microsoft.Expression.Controls;

namespace SilverlightApplication6
{
    public class GraphDrawer
    {
        /*constructor get a list of nodes*/
        public List<Node> nodes;
		public Canvas dfaDiplay;
		static int i = 0;

        public GraphDrawer(List<Node> nodes, Canvas c)
        {
            this.nodes = nodes;
			dfaDiplay = new Canvas();
			c.Children.Add(dfaDiplay);
        }

        public void drawNodes(/*Canvas c*/)
		/* draw all nodes on the Canvas c */
        {
            foreach ( Node i in nodes)
            {
				dfaDiplay.Children.Add(i.presentor);
				/* postioning */
				justifyPostion(i);
            }
        }

		public void drawEdges()
		/* draw edges on the Canvas c */
		{
			foreach (Node i in nodes)
			{
				List<Tuple<Node,string>> adjacent = i.adjacent;
				foreach ( Tuple<Node,string> j in adjacent)
				{
					/*Tuple<Point, Point, Point, Point> coordinate = computeEdge(i, j.Item1);
					constructBezier(coordinate);
					 */
				}
			}
		}

		private void constructBezier(Tuple<Point, Point, Point, Point> coordinate)
		{
			
			var bz = new BezierSegment();
			//var p1 = new Point(100, 30);
			//bz.Point1 = p1;
			bz.Point1 = coordinate.Item2;
			//var p2 = new Point(200, 30);
			//bz.Point2 = p2;
			bz.Point2 = coordinate.Item3;
			var p3 = new Point(300, -40);
			bz.Point3 = p3;
			//bz.Point3 = coordinate.Item4;

			var pc = new PathSegmentCollection();
			pc.Add(bz);

			var pf = new PathFigure();
			pf.StartPoint = new Point(0, 0);
			//pf.StartPoint = coordinate.Item1;
			pf.Segments = pc;
			var pfc = new PathFigureCollection();
			pfc.Add(pf);
			var pg = new PathGeometry();
			pg.Figures = pfc;

			var path = new Path();
			path.Data = pg;
			path.Stroke = new SolidColorBrush(Colors.Black);

			dfaDiplay.Children.Add(path);
			path.SetValue(Canvas.TopProperty,i+0.0);
			i += 10;
			
		}

		public void justifyPostion(Node i)
		/*justify the position of the node i in canvas*/
		{
			i.presentor.SetValue(Canvas.TopProperty, i.y);
			i.presentor.SetValue(Canvas.LeftProperty, i.x);
		}

		//public Tuple<Point, Point, Point, Point> computeEdge(Node i, Node j)
		public Tuple<EPoint, EPoint, EPoint, EPoint> computeEdge(Node i, Node j, TextBox log)
		/* compute 4 point to draw  */
		{
			Point beginPoint;
			Point endPoint;
			Point m1Point;
			Point m2Point;

			EPoint begin = new EPoint(i.x, -i.y);
			
			log.Text += "begin: x" + begin.x + "\n";
			log.Text += "     : y" + begin.y + "\n";
			

			EPoint end = new EPoint(j.x, -j.y);
			/*
			log.Text += "end: x" + end.x + "\n";
			log.Text += "   : y" + end.y + "\n";
			*/

			/* move the coordinate base to begin */
			EVector transform = new EVector(i.x + i.presentor.RenderSize.Width/2 , 
				- (i.y + i.presentor.RenderSize.Height/2));
			/*
			log.Text += "transform vector: x " + transform.x + "\n";
			log.Text += "                : y " + transform.y + "\n";
			*/
			begin.transformCoordinate(transform);
			

			
			log.Text += "after transform:\n";
			log.Text += "begin: x " + begin.x + "\n";
			log.Text += "     : y " + begin.y + "\n";
			

			end.transformCoordinate(transform);
			/*
			log.Text += "end: x" + end.x + "\n";
			log.Text += "   : y" + end.y + "\n";
			*/

			/* rotate the coordinate, so that the vector <begin->end> is the x-axes */
			EVector v1 = new EVector(1, 0);
			EVector v2 = new EVector(begin, end);
			
			begin.rotateCoordinate(v1, v2);
			
			
			log.Text += "after rotation:\n";
			log.Text += "begin: x" + begin.x + "\n";
			log.Text += "     : y" + begin.y + "\n";
			
			
			end.rotateCoordinate(v1, v2);
			
			/*
			log.Text += "end: x" + end.x + "\n";
			log.Text += "   : y" + end.y + "\n";
			*/

			/* compute the points */
			EPoint p1;
			EPoint p2;
			EPoint p3;
			EPoint p4;
			double alpha;
			var distance = begin.distanceTo(end);
			/*TODO: calibiert Fehlertolranz*/
			if (end.x > 0)
			{
				alpha = Math.PI / 6;
				log.Text += "alpha: " + alpha + "\n";
				
				p1 = new EPoint(i.presentor.RenderSize.Width * Math.Cos(alpha),
					i.presentor.RenderSize.Width * Math.Sin(alpha));
				
				p2 = new EPoint(distance / 3,
					distance / 3);
				p3 = new EPoint(2 * distance / 3,
					distance / 3);
				p4 = new EPoint(distance - j.presentor.RenderSize.Width * Math.Cos(alpha),
					j.presentor.RenderSize.Width * Math.Sin(alpha));
			}else
			{
				alpha = Math.PI  + Math.PI / 6;
				p1 = new EPoint(i.presentor.RenderSize.Width * Math.Cos(alpha),
					i.presentor.RenderSize.Width * Math.Sin(alpha));
				p2 = new EPoint(- distance / 3,
					- distance / 3);
				p3 = new EPoint(- 2 * distance / 3,
					- distance / 3);
				p4 = new EPoint(- distance - j.presentor.RenderSize.Width * Math.Cos(alpha),
					j.presentor.RenderSize.Width * Math.Sin(alpha));
			}
			/* transform backward */
			log.Text += "before bw transform:\n";
			log.Text += "p1: x " + p1.x + "\n";
			log.Text += "  : y " + p2.x + "\n";

			p1.rotateCoordinate(v2, v1);
			log.Text += "after bw rotation:\n";
			log.Text += "p1: x " + p1.x + "\n";
			log.Text += "  : y " + p2.x + "\n";

			p1.transformCoordinate(-transform);
			p2.rotateCoordinate(v2, v1);
			p2.transformCoordinate(-transform);
			p3.rotateCoordinate(v2, v1);
			p3.transformCoordinate(-transform);
			p4.rotateCoordinate(v2, v1);
			p4.transformCoordinate(-transform);
			
			/* create Silverlight Point  */
			beginPoint = p1.makeSPoint();
			m1Point = p2.makeSPoint();
			m2Point = p3.makeSPoint();
			endPoint = p4.makeSPoint();
			log.Text += "result:\n";
			log.Text += "begin point: x " + beginPoint.X + "\n";
			log.Text += "           : y " + beginPoint.Y + "\n";
			return new Tuple<EPoint, EPoint, EPoint, EPoint>(p1, p2, p3, p4);
			//return new Tuple<Point, Point, Point, Point>(beginPoint, m1Point, m2Point, endPoint);
		}
    }
}
