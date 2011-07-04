using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverlightApplication6;
using System.Windows.Controls;
using Microsoft.Expression.Controls;
using System.Net;

using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace DummyProject
{
	class Program
	{
		static void Main(string[] args)
		{
			Node q1 = new Node("q1", 0, 0, false);
			Node q2 = new Node("q2", 100, 0, true);

			Tuple<EPoint, EPoint, EPoint, EPoint> points = LayoutComputer.computeEdge(q1, q2);
			Console.Write("result:\n");
			Console.Write("p1.x " + points.Item1.x + "\n");
			Console.Write("p1.y " + points.Item1.y + "\n");
			Console.Write("p4.x " + points.Item4.x + "\n");
			Console.Write("p4.y " + points.Item4.y + "\n");
			
			Console.ReadKey();
		}
	}
}
