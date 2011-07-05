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
			one.addAdjacent(three, "b");
			two.addAdjacent(three, "a");
			two.addAdjacent(four, "b");
			four.addAdjacent(one, "b");
			four.addAdjacent(three, "a");
			

			List <Node> dummyDFA = new List<Node> ();
			dummyDFA.Add(one);
			dummyDFA.Add(two);
			dummyDFA.Add(three);
			dummyDFA.Add(four);

			LayoutComputer.computeEdge(one, two);
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
