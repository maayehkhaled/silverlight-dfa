using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SilverlightApplication6
{
	class LayoutComputer
	{
		public static Tuple<EPoint, EPoint, EPoint, EPoint> computeEdge(Node i, Node j)
		{
			EPoint begin = new EPoint(i.x+i.width/2, - (i.y+i.height/2) );
			EPoint end = new EPoint(j.x+j.width/2, - (j.y+j.height/2) );

			
			/* move the coordinate base to begin */
			EVector transform = new EVector(i.x + i.width / 2,
				-(i.y + i.height / 2));

			begin.transformCoordinate(transform);
			end.transformCoordinate(transform);
			

			/* rotate the coordinate, so that the vector <begin->end> is the x-axes */
			EVector v1 = new EVector(1, 0);
			EVector v2 = new EVector(begin, end);

			begin.rotateCoordinate(v1, v2);
			Console.Write("After rotation\n");
			Console.Write("begin.x " + begin.x + "\n");// 0
			Console.Write("begin.y " + begin.y + "\n");// 0
			end.rotateCoordinate(v1, v2);
			Console.Write("  end.x " + end.x + "\n");// 100
			Console.Write("  end.y " + end.y + "\n");// 0
			
			/* compute the points */
			EPoint p1;
			EPoint p2;
			EPoint p3;
			EPoint p4;
			double alpha;
			var distance = begin.distanceTo(end);
			Console.Write("distance: " + distance + "\n");
			/*TODO: calibiert Fehlertolranz*/
			if (end.x > 0)
			{
				alpha = Math.PI / 6;
				p1 = new EPoint(i.width/2 * Math.Cos(alpha),
					i.height/2 * Math.Sin(alpha));
				p2 = new EPoint(distance / 3,
					distance / 3);
				p3 = new EPoint(2 * distance / 3,
					distance / 3);
				p4 = new EPoint(distance - j.width/2 * Math.Cos(alpha),
					j.height/2 * Math.Sin(alpha));
			}
			else
			{
				alpha = Math.PI + Math.PI / 6;
				p1 = new EPoint(i.width/2 * Math.Cos(alpha),
					i.height/2 * Math.Sin(alpha));
				p2 = new EPoint(-distance / 3,
					-distance / 3);
				p3 = new EPoint(-2 * distance / 3,
					-distance / 3);
				p4 = new EPoint(- distance - j.width/2 * Math.Cos(alpha),
					j.height/2 * Math.Sin(alpha));
			}
			Console.Write("after compute\n");
			Console.Write("p1.x " + p1.x + "\n");// 10.82
			Console.Write("p1.y " + p1.y + "\n");// 6.25
			Console.Write("p4.x " + p4.x + "\n");// 89.93
			Console.Write("p4.y " + p4.y + "\n");// 6.25


			/* transform backward */
			p1.rotateCoordinate(v2, v1);
			Console.Write("after compute rotation\n");
			Console.Write("    p1.x " + p1.x + "\n");// 10.82
			Console.Write("    p1.y " + p1.y + "\n");// 6.25
			p4.rotateCoordinate(v2, v1);
			Console.Write("    p4.x " + p4.x + "\n");// 89.93
			Console.Write("    p4.y " + p4.y + "\n");// 6.25

			p1.transformCoordinate(-transform);
			Console.Write("after compute transform\n");
			Console.Write("    p1.x " + p1.x + "\n");// 10.82
			Console.Write("    p1.y " + p1.y + "\n");// 6.25
			p4.transformCoordinate(-transform);

			Console.Write("    p4.x " + p4.x + "\n");// 101
			Console.Write("    p4.y " + p4.y + "\n");// 6.25

			p2.rotateCoordinate(v2, v1);
			p2.transformCoordinate(-transform);

			p3.rotateCoordinate(v2, v1);
			p3.transformCoordinate(-transform);

			
			

			/* create Silverlight Point  */
			
			return new Tuple<EPoint, EPoint, EPoint, EPoint>(p1, p2, p3, p4);
		}
	}
}
