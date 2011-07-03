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

		public void drawEdges(Canvas c)
		/* draw edges on the Canvas c */
		{
			foreach (Node i in nodes)
			{
				List<Tuple<Node,string>> adjacent = i.adjacent;
				foreach ( Tuple<Node,string> j in adjacent)
				{
					Tuple<Point, Point, Point, Point> coordinate = computeEdge(i, j.Item1);
					constructBezier(coordinate);
				}
			}
		}

		private void constructBezier(Tuple<Point, Point, Point, Point> coordinate)
		{
			
		}

		public void justifyPostion(Node i)
		/*justify the position of the node i in canvas*/
		{
			i.presentor.SetValue(Canvas.TopProperty, i.y);
			i.presentor.SetValue(Canvas.LeftProperty, i.x);
		}

		public Tuple<Point, Point, Point, Point> computeEdge(Node i, Node j)
		/* compute 4 point to draw  */
		{
			Point beginPoint = new Point();
			Point endPoint = new Point();
			Point m1Point = new Point();
			Point m2Point = new Point();


			return new Tuple<Point, Point, Point, Point>(beginPoint, m1Point, m2Point, endPoint);
		}
    }
}
