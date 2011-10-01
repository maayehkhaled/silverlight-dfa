using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;


namespace SilverlightApplication6
{
	public static class LayoutComputer
	{

        public static Tuple<EPoint, EPoint, EPoint, EPoint> computeEdge(VisualNode i, VisualNode j)
		{
			EPoint begin = new EPoint(
				i.location.x + i.getGrid().Width / 2,
                -(i.location.y + i.getGrid().Height / 2)
			);
			EPoint end = new EPoint(
                j.location.x + j.getGrid().Width / 2,
                -(j.location.y + j.getGrid().Height / 2)
			);
			/* move the coordinate base to begin */
			EVector transform = new EVector(
                i.location.x + i.getGrid().Width / 2,
                -(i.location.y + i.getGrid().Height / 2)
			);
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
			var minimalDistance = 0;
			if (distance > minimalDistance)
			{
				var xFactor = 3;
				var yFactor = 4;
				if (end.x > 0)
				{
					alpha = Math.PI / 6;
                    p1 = new EPoint(i.getGrid().Width / 2 * Math.Cos(alpha),
                        i.getGrid().Height / 2 * Math.Sin(alpha));
					p2 = new EPoint(distance / xFactor,
						distance / yFactor);
					p3 = new EPoint(2 * distance / xFactor,
						distance / yFactor);
                    p4 = new EPoint(distance - j.getGrid().Width / 2 * Math.Cos(alpha),
                        j.getGrid().Height / 2 * Math.Sin(alpha));
				
				}else
				{
					alpha = Math.PI + Math.PI / 6;
                    p1 = new EPoint(i.getGrid().Width / 2 * Math.Cos(alpha),
                        i.getGrid().Height / 2 * Math.Sin(alpha));
					p2 = new EPoint(-distance / xFactor,
						-distance / yFactor);
					p3 = new EPoint(-2 * distance / xFactor,
						-distance / yFactor);
                    p4 = new EPoint(-distance - j.getGrid().Width / 2 * Math.Cos(alpha),
                        j.getGrid().Height / 2 * Math.Sin(alpha));
				}
			}else
			{
				var xFactor = 3;
				var yFactor = 4;
				alpha = Math.PI / 3;
                //Debug.WriteLine("*******************Math.Cos(alpha): " + Math.Cos(alpha));
                p1 = new EPoint(i.getGrid().Width / 2 * Math.Cos(alpha),
                    i.getGrid().Height / 2 * Math.Sin(alpha));
                //Debug.WriteLine("*******************before transform: p1 ");
                //Debug.WriteLine("*******************      x: " + p1.x);
                //Debug.WriteLine("*******************      y: " + p1.y);

                p2 = new EPoint(xFactor * i.getGrid().Width / 2 * Math.Cos(alpha),
                    yFactor * i.getGrid().Height / 2 * Math.Sin(alpha));

                p3 = new EPoint(xFactor * (distance - j.getGrid().Width / 2 * Math.Cos(alpha)),
                    yFactor * (j.getGrid().Height / 2 * Math.Sin(alpha)));

                p4 = new EPoint(distance - j.getGrid().Width / 2 * Math.Cos(alpha),
                    j.getGrid().Height / 2 * Math.Sin(alpha));
			}

			/* transform backward */
			p1.rotateCoordinate(v2, v1);
            //Debug.WriteLine("*******************after rotation: p1 ");
            //Debug.WriteLine("*******************      x: " + p1.x);
            //Debug.WriteLine("*******************      y: " + p1.y);
			p2.rotateCoordinate(v2, v1);
			p3.rotateCoordinate(v2, v1);
			p4.rotateCoordinate(v2, v1);

			p1.transformCoordinate(-transform);
			p2.transformCoordinate(-transform);
			p3.transformCoordinate(-transform);
			p4.transformCoordinate(-transform);
			/* create Silverlight Point  */
			p1.y = -p1.y;
			p2.y = -p2.y;
			p3.y = -p3.y;
			p4.y = -p4.y;
            //Debug.WriteLine("*******************Point 1:");
            //Debug.WriteLine("*******************      x: " + p1.x);
            //Debug.WriteLine("*******************      y: " + p1.y);
            //Debug.WriteLine("*******************Point 4:");
            //Debug.WriteLine("*******************      x: " + p4.x);
            //Debug.WriteLine("*******************      y: " + p4.y);
			return new Tuple<EPoint, EPoint, EPoint, EPoint>(p1, p2, p3, p4);
		}

		public static Tuple<EPoint, EPoint> computeArrow(EPoint target, EPoint direction)
		{
			EPoint target2 = new EPoint(target.x, -target.y);
			EPoint direction2 = new EPoint(direction.x, -direction.y);
			EVector transformer = new EVector(target2);
			target2.transformCoordinate(transformer);
			direction2.transformCoordinate(transformer);

			EVector v1 = new EVector(1, 0);
			EVector v2 = new EVector(target2, direction2);
			target2.rotateCoordinate(v1, v2);
			direction2.rotateCoordinate(v1, v2);

			var angel = Math.PI / 6;
			var radius = 10;
			EPoint begin = new EPoint(radius * Math.Cos(angel), radius * Math.Sin(angel));
			EPoint end = new EPoint(begin.x, -begin.y);

			begin.rotateCoordinate(v2, v1);
			end.rotateCoordinate(v2, v1);

			begin.transformCoordinate(-transformer);
			end.transformCoordinate(-transformer);
			
			begin.y = -begin.y;
			end.y = -end.y;

			return new Tuple<EPoint, EPoint>(begin, end);
		}
	}
}
