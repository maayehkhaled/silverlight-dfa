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
        /*constructor get a list of nodes*/
        public List<Node> nodes;

        public GraphDrawer(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public void drawNodes(Canvas c)
        {
            foreach ( Node i in nodes)
            {
				c.Children.Add(i.presentor);
				i.presentor.SetValue(Canvas.TopProperty, i.y);
				i.presentor.SetValue(Canvas.LeftProperty, i.x);
            }
        }

		private void setPosition(Button nodePresentor,Node i)
		{
			/*simplst case: use direct the infor from Node i without any computation*/
			nodePresentor.SetValue(Canvas.TopProperty, i.y);
			nodePresentor.SetValue(Canvas.LeftProperty, i.x);
		}

    }
}
