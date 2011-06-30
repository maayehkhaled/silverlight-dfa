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
<<<<<<< .mine
//using QuickGraph;
//using QuickGraph.Algorithms;
//using QuickGraph.Collections;
=======
>>>>>>> .r9
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


            //<ed:LineArrow Fill="#FFF4F4F5" Height="100" Canvas.Left="202" Stroke="Black" Canvas.Top="57" Width="100" RenderTransformOrigin="0.5,0.5" UseLayoutRounding="False" d:LayoutRounding="Auto">
            LineArrow a = new LineArrow();
            a.Height = 100;
            a.Width = 100;
            a.Stroke = new SolidColorBrush(Color.FromArgb(0xD0, 0xD0, 0xD0, 0xD0));
            a.StrokeThickness = 2;
           
            canvas1.Children.Add(a);



            Ellipse e1 = new Ellipse();
            e1.Height = 40;
            e1.Width = 40;
            e1.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x3D, 0xE3, 0x18));
            //e1.
            canvas1.Children.Add(e1);

            //e1.Parent = 

            ////Ellipse e2 = new Ellipse();
            ////e2.Height = 40;
            ////e2.Width = 40;
            ////e2.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x3D, 0xE3, 0x18));



            //canvas1.UpdateLayout();
            //LayoutRoot.UpdateLayout();
            //LayoutRoot.FindName("canvas1");
        }
    }
}
