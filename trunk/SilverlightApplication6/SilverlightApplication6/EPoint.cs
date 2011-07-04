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
			x = x - v.x;
			y = y - v.y;
		}

		public void rotateCoordinate(EVector v1, EVector v2)
		/* compute the the coordinate in new rotated coordination system.
		 * New Coordinate system is the result of rotating the coordinate system 
		 * of a angle phi, cos(phi) = (v1 * v2) / abs(a) * abs(b)
		 * if the orientation of v1 and v2 positiv (uhrzeigersinn) the rotation angel
		 * is positive, else negativ.
		 */
		{
			var phi = Math.Acos( v1 * v2 / (v1.abs() * v2.abs()) );
			if (v1.orientationWith(v2) < 0)
			{
				phi = -phi;
			}
			rotateCoordinate(phi);
		}

		public void rotateCoordinate(double phi)
		{
			double newX, newY;
			newX = x * Math.Cos(phi) + y * Math.Sin(phi);
			newY = -x * Math.Sin(phi) + y * Math.Cos(phi);
			x = newX; y = newY;
		}

		public double distanceTo(EPoint p)
		{
			return Math.Sqrt( (x - p.x)*(x-p.x) + (y-p.y)*(y-p.y) );
		}
		public Point makeSPoint()
		{
			return new Point(x, -y);
		}
	}

	public class EVector
	{
		public double x = 0;
		public double y = 0;
		public EVector(EPoint begin, EPoint end)
		{
			x = end.x - begin.x;
			y = end.y - begin.y;
		}
		public EVector(EPoint free)
		{
			x = free.x;
			y = free.y;
		}
		public EVector()
		{
			x = 0.0; 
			y = 0.0;
		}
		public EVector(double x, double y)
		{
			this.x = x; this.y = y;
		}

		public static double operator *(EVector v, EVector u)
		/*compute scale production of v und u*/
		{
			return v.scaleProd(u);
		}

		public double scaleProd(EVector v)
		/* compute scale production of v and this */
		{
			double X = x * v.x;
			double Y = y * v.y;
			return X + Y;
		}

		public double orientationWith(EVector v)
		/* compute the "cross production" this [x] v, assume all z's coordiante equals 0 */
		{
			return x * v.y - y * v.x;
		}


		public double angleWith(EVector v)
		/* return the cos of the angle between vector and v */
		{
			return  (this * v ) / (abs() *v.abs()) ;
		}
		public double abs()
		{
			return Math.Sqrt(x * x + y * y);
		}

		public static EVector operator -(EVector v)
		{
			v.x = -v.x;
			v.y = -v.y;
			return v;
		}
	}
}
