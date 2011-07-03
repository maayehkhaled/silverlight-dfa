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

        public GraphDrawer(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public void drawNodes(Canvas c)
		/* draw all nodes on the Canvas c */
        {
            foreach ( Node i in nodes)
            {
				c.Children.Add(i.presentor);
				/* postioning */
				justifyPostion(i);
            }
        }

		public void drawEdges(Canvas c)
		/* draw edges on the Canvas c */
		{
			foreach (Node i in nodes)
			{
				List<Tuple<Node,string>> outNode = i.adjacent;
				foreach ( Node j in outNode)
				{
					Tuple<double, double, double, double> coordinate = computeEdge(i, j);

				}
			}
		}

		public void justifyPostion(Node i)
		{
			i.presentor.SetValue(Canvas.TopProperty, i.y);
			i.presentor.SetValue(Canvas.LeftProperty, i.x);
		}

    }
}
