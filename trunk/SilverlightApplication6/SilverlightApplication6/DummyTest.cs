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
		public void drawDummyDFA(Canvas c)
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
			
			one.addAdjacent(two, "a");
			one.addAdjacent(one, "b");
			two.addAdjacent(one, "c");

			List <Node> dummyDFA = new List<Node> ();
			dummyDFA.Add(one);
			dummyDFA.Add(two);
			dummyDFA.Add(three);
			dummyDFA.Add(four);

			GraphDrawer gd = new GraphDrawer(dummyDFA,c);
			gd.drawDFA();
			
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

		public void PrintArrow(Canvas c)
		{
			Path arrow = new Path();
			arrow.Width = 56.6147;
			arrow.Height = 56.6146;
			arrow.Stretch = Stretch.Fill;

			//arrow.Data = ArrowFactory.createArrow(ArrowType.Arrow_27);
			var datastring = "F1 M 77.2798,42.312L 62.3771,30.448L 57.4118,36.6814L 65.8771,43.4214L 42.6131,43.4214L 42.6131,51.392L 65.8745,51.392L 57.4118,58.1294L 62.3771,64.3654L 77.2798,52.5L 83.6771,47.4053L 77.2798,42.312 Z ";
			var geodata = new PathGeometry();
			geodata.SetValue(System.Windows.Shapes.Path.DataProperty, datastring);

			SolidColorBrush edgeColor = new SolidColorBrush(Colors.Brown);
			arrow.Stroke = edgeColor;
			arrow.Stroke = edgeColor;
			c.Children.Add(arrow);
			

			arrow.SetValue(Canvas.TopProperty, 0.0);
			arrow.SetValue(Canvas.LeftProperty, 0.0);
		}
		
    }
}
