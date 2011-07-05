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
    public class DummyTest
    /*
	 * this class is created to use to quick and dirty debug
	 */
    {
		public void makeSmall(Canvas c) { c.Height = c.Height / 2; }
		public void drawDummyDFA(Canvas c, TextBox log)
		{
			/*
			 * >1 -> 2 [a]
			 * 1 -> 3 [b]
			 * 2 -> 3 [a]
			 * 2 -> 4 [b]
			 * 4 -> 1 [b]
			 * <4 -> 3 [a]
 			 */
			GenerateGrid(c, 25.0);
			Node one = new Node("1", 0,   0, false);
			Node two = new Node("2", 100, 0, false);
			Node three = new Node("3", 0, 100, false);
			Node four = new Node("4", 100, 100, true);
			
			one.addAdjcent(two, "a");
			/*one.addAdjcent(three, "b");
			two.addAdjcent(three, "a");
			two.addAdjcent(four, "b");
			four.addAdjcent(one, "b");
			four.addAdjcent(three, "a");
			*/

			List <Node> dummyDFA = new List<Node> ();
			dummyDFA.Add(one);
			dummyDFA.Add(two);
			dummyDFA.Add(three);
			dummyDFA.Add(four);

			
			//gd.drawEdges();
			/*Tuple<EPoint,EPoint,EPoint,EPoint> coordinate = gd.computeEdge(one, two, log);
			log.Text += "p1, x: " + coordinate.Item1.x + "\n";
 			log.Text += "p1, y: " + coordinate.Item1.y + "\n";
			*/

			/*
			LineArrow x = new LineArrow();
			x.Width = 100; x.Height = 100;

			x.Stroke = new SolidColorBrush(Colors.Black);
			x.StrokeThickness = 2;
			x.Opacity = 0.5;
			
			

			RotateTransform t = new RotateTransform();
			t.Angle=-180;
			t.CenterX = x.Width/2;
			t.CenterY = x.Height / 2;
			x.RenderTransform = t;

			c.Children.Add(x);
			x.SetValue(Canvas.TopProperty, 20.0);
			x.SetValue(Canvas.LeftProperty, 50.0);

			
		
			var bz = new BezierSegment();
			var p1 = new Point(100,30);
			bz.Point1 = p1;
			var p2 = new Point(200,30);
			bz.Point2 = p2;
			var p3 = new Point(300,-40);
			bz.Point3 = p3;
			
			var pc = new PathSegmentCollection();
			pc.Add(bz);

			var pf = new PathFigure();
			pf.StartPoint = new Point(0, 0);
			pf.Segments = pc;
			var pfc = new PathFigureCollection();
			pfc.Add(pf);
			var pg = new PathGeometry();
			pg.Figures = pfc;

			var path = new Path();
			path.Data = pg;
			path.Stroke = new SolidColorBrush(Colors.Black);

 			c.Children.Add(path);
			//* now move bz 
			path.SetValue(Canvas.TopProperty, 0.0);
			path.SetValue(Canvas.LeftProperty, 0.0);
			*/
		}

		private void GenerateGrid(Canvas panel, double gap)
		{
			double row = panel.Height / gap;
			double col = panel.Width / gap;

			for (int i = 0; i <= Convert.ToInt32(row); i++)
			{
				StackPanel sp = new StackPanel();
				sp.Orientation = System.Windows.Controls.Orientation.Horizontal;

				for (int j = 0; j <= Convert.ToInt32(col); j++)
				{
					sp.Children.Add(new Border { BorderBrush = new SolidColorBrush(Colors.Gray), 
						BorderThickness = new Thickness(.1), Width = gap, Height = gap });
				}

				panel.Children.Add(sp);
				sp.SetValue(Canvas.TopProperty, i * gap);

			}
		}


		public void testVector(TextBox log)
		{
			log.Text = "log\n";
			EVector v1 = new EVector(0, 3);
			EVector v2 = new EVector(0, 2);

			EPoint p1 = new EPoint(3,4);

 			p1.rotateCoordinate(v1,v2);
			log.Text +="p1.x" + p1.x + "\n";
			log.Text +="p1.y" + p1.y + "\n";

		}
    }
}
