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
			EPoint begin = new EPoint(i.x, -i.y);
			EPoint end = new EPoint(j.x, -j.y);
			
			/* move the coordinate base to begin */
			EVector transform = new EVector(i.x + i.width / 2,
				-(i.y + i.height / 2));
			
			begin.transformCoordinate(transform);
			
			end.transformCoordinate(transform);
			
			/* rotate the coordinate, so that the vector <begin->end> is the x-axes */
			EVector v1 = new EVector(1, 0);
			EVector v2 = new EVector(begin, end);

			begin.rotateCoordinate(v1, v2);


			
			end.rotateCoordinate(v1, v2);

			
			/* compute the points */
			EPoint p1;
			EPoint p2;
			EPoint p3;
			EPoint p4;
			double alpha;
			var distance = begin.distanceTo(end);
			/*TODO: calibiert Fehlertolranz*/
			if (end.x > 0)
			{
				alpha = Math.PI / 6;
				

				p1 = new EPoint(i.width * Math.Cos(alpha),
					i.width * Math.Sin(alpha));

				p2 = new EPoint(distance / 3,
					distance / 3);
				p3 = new EPoint(2 * distance / 3,
					distance / 3);
				p4 = new EPoint(distance - j.width * Math.Cos(alpha),
					j.width * Math.Sin(alpha));
			}
			else
			{
				alpha = Math.PI + Math.PI / 6;
				p1 = new EPoint(i.width * Math.Cos(alpha),
					i.width * Math.Sin(alpha));
				p2 = new EPoint(-distance / 3,
					-distance / 3);
				p3 = new EPoint(-2 * distance / 3,
					-distance / 3);
				p4 = new EPoint(-distance - j.width * Math.Cos(alpha),
					j.width * Math.Sin(alpha));
			}
			/* transform backward */
			

			p1.rotateCoordinate(v2, v1);
			

			p1.transformCoordinate(-transform);
			p2.rotateCoordinate(v2, v1);
			p2.transformCoordinate(-transform);
			p3.rotateCoordinate(v2, v1);
			p3.transformCoordinate(-transform);
			p4.rotateCoordinate(v2, v1);
			p4.transformCoordinate(-transform);

			/* create Silverlight Point  */
			
			return new Tuple<EPoint, EPoint, EPoint, EPoint>(p1, p2, p3, p4);
		}
	}
}
