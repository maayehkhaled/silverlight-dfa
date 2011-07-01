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
			Node one = new Node("1", 0,   0, false);
			Node two = new Node("2", 100, 0, false);
			Node three = new Node("3", 0, 100, false);
			Node four = new Node("4", 100, 100, true);
			
			one.addAdjcent(two, "a");
			one.addAdjcent(three, "b");
			two.addAdjcent(three, "a");
			two.addAdjcent(four, "b");
			four.addAdjcent(one, "b");
			four.addAdjcent(three, "a");

			List <Node> dummyDFA = new List<Node> ();
			dummyDFA.Add(one);
			dummyDFA.Add(two);
			dummyDFA.Add(three);
			dummyDFA.Add(four);

			GraphDrawer gd = new GraphDrawer(dummyDFA);
			gd.drawNodes(c);

			/*
			string x = "flip";
			string y = "flip";
			if (x.Equals(y))
			{
				TextBlock tb = new TextBlock(); tb.Text = "Equals";
				c.Children.Add(tb);
			}
			*/

			LineArrow x = new LineArrow();
			x.Width = 100; x.Height = 100;
			string y = "a";
			Tuple<LineArrow, string> z = new Tuple<LineArrow, string>(x, y);
			var newStr = z.Item2 + "|" + "b";
			
			


		}

    }
}
