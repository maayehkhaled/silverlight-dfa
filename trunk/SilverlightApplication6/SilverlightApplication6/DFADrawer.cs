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
        public List<Button> buttons;

        const double NODE_WIDTH = 100.0;
        const double NODE_HEIGHT = 100.0;

        public GraphDrawer(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public void drawNodes(Canvas c)
        {
            foreach ( Node i in nodes)
            {
                /*construct graphic element, which presents the node on cavas*/
                Button nodePresentor = createButton(i);
            }
        }

        private Button createButton(Node i)
        {
            Button button = new Button();
            button.Content = i.nodeLabel;
            button.Width = NODE_WIDTH;
            button.Height = NODE_HEIGHT;
            return button;
        }
    }
}
