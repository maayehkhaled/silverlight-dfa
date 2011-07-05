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
	public class VisualNode
	{
		public readonly Node node;
		public Grid state;
		public List<Grid> outEdges;

		static SolidColorBrush normalState = new SolidColorBrush(Colors.Blue);
		static SolidColorBrush endState = new SolidColorBrush(Colors.Red);
		static SolidColorBrush border = new SolidColorBrush(Colors.Black);
		static SolidColorBrush edgeColor = new SolidColorBrush(Colors.Brown);
		

		public VisualNode(Node n)
		{
			node = n;
			state = new Grid();
			state.Width = node.width;
			state.Height = node.height;

			Ellipse e = new Ellipse();
			e.Width = node.width;
			e.Height = node.height;

			if (node.isEnd)
				e.Fill = endState;
			else
				e.Fill = normalState;

			e.StrokeThickness = 2;
			e.Stroke = border;

			state.Children.Add(e);

			TextBlock label = new TextBlock();
			label.Text = node.nodeLabel;
			state.Children.Add(label);
			label.TextAlignment = TextAlignment.Center;
			label.VerticalAlignment = VerticalAlignment.Center;

			/*
			foreach (Tuple<Node,string> m in node.adjacent)
			{
				createOutEdge(m);
			}
			*/
		}

		/*
		
		*/

		public void catchSymbol()
		{
		}

		public void throwSymbol()
		{
		}

		
	}

}


/*
 * 
 BezierSegment bz = new BezierSegment();
			bz.Point1 = new Point(bezierPoints.Item2.x, bezierPoints.Item2.y);
			bz.Point2 = new Point(bezierPoints.Item3.x, bezierPoints.Item3.y);
			bz.Point3 = new Point(bezierPoints.Item4.x, bezierPoints.Item4.y);

			PathSegmentCollection psCollection = new PathSegmentCollection();
			psCollection.Add(bz);

			PathFigure bezierFigure = new PathFigure();
			bezierFigure.StartPoint = new Point(bezierPoints.Item1.x, bezierPoints.Item1.y);
			bezierFigure.Segments = psCollection;

			PathFigureCollection pathFigCollection = new PathFigureCollection();
			pathFigCollection.Add(bezierFigure);

			PathGeometry pathGeo = new PathGeometry();
			pathGeo.Figures = pathFigCollection;

			Path path = new Path();
			path.Data = pathGeo;
			path.Stroke = edgeColor;
*/