using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Collections;
using Microsoft.Expression.Controls;

namespace SilverlightApplication6
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            //var g = new AdjacencyGraph<int, TaggedEdge<int, string>>();
            //g.AddVertex(3);
            //var populator = GleeGraphExtensions.Create<string, Edge<string>>(g);
            //populator.Compute();
            //Graph g = populator.GleeGraph;
            //IVertexAndEdgeListGraph<string, Edge<string>> g = DelegateVertexAndEdgeListGraph graph;

            LineArrow a = new LineArrow();
            Canvas.SetZIndex(a, 1);
            canvas1.Children.Add(a);
            //canvas1.UpdateLayout();
            //LayoutRoot.UpdateLayout();
            //LayoutRoot.FindName("canvas1");
        }
    }
}
