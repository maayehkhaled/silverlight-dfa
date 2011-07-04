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
			
			EPoint p1 = new EPoint(25/2, -25/2);
			
			EVector transform = new EVector(25 / 2, -25 / 2);
			p1.transformCoordinate(transform);
			Console.Write("after transform:\n");
			Console.Write("                p1.x " + p1.x + "\n");
			Console.Write("                p1.y " + p1.y + "\n");
			Console.ReadKey();
		}
	}
}
