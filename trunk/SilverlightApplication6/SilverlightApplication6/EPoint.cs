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

namespace SilverlightApplication6
{
	public class EPoint
	/* use normal coordinate system (standard Euclid Coordinate System) */
	{
		public double x = 0;
		public double y = 0;

		public EPoint(double x, double y)
		{
			this.x = x; this.y = y;
		}
		public EPoint():this(0.0,0.0){}
		public EPoint(Point p) : this(p.X, -p.Y) { }

		public void transformCoordinate(EVector v)
		/* compute the new coordinate in new transform coordiante system. 
		 * New coordianate system is the result of moving the old coordinate system 
		 * along the vector v
		 */
		{
			this.x = this.x - v.x;
			this.y = this.y - v.y;
		}

		public void rotateCoordinate(EVector v1, EVector v2)
		/* compute the the coordinate in new rotated coordination system.
		 * New Coordinate system is the result of rotating the coordinate system 
		 * of a angle phi, cos(phi) = (v1 * v2) / abs(a) * abs(b)
		 */
		{
			var phi = Math.Acos( v1 * v2 / (v1.abs() * v2.abs()) );
			rotateCoordinate(phi);
		}

		public void rotateCoordinate(double phi)
		{
			double newX, newY;
			newX = x * Math.Cos(phi) + y * Math.Sin(phi);
			newY = -x * Math.Sin(phi) + y * Math.Cos(phi);
			x = newX; y = newY;
		}
	}

	public class EVector
	{
		public double x = 0;
		public double y = 0;
		public EVector(EPoint begin, EPoint end)
		{
			this.x = end.x - begin.x;
			this.y = end.y - begin.y;
		}
		public EVector(EPoint free)
		{
			this.x = free.x;
			this.x = free.y;
		}
		public EVector()
		{
			this.x = 0.0; this.y = 0.0;
		}

		public static double operator *(EVector v, EVector u)
		/*compute scale production of v und u*/
		{
			double X = v.x * u.x;
			double Y = v.y * u.y;
			return Math.Sqrt(X * X + Y * Y);
		}

		public double abs()
		{
			return Math.Sqrt(x * x + y * y);
		}
	}
}
