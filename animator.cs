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
	public class Animator
	{
        public static Storyboard createDestinationNodeAnimation(VisualNode vn, Storyboard storyboard)
        {
            DoubleAnimation animation = new DoubleAnimation();

            BounceEase y = new BounceEase();
            y.Bounces = 3;
            y.Bounciness = 1.5;

            BounceEase x = new BounceEase();
            x.Bounces = 3;
            x.Bounciness = 1.5;

            return storyboard;
        }

		/*some settings*/
		/* how many seconds take the animation for catch Symbol event */
		private static double _nodeCatcheSymbol = 1;
		public static double nodeCatcheSymbol
		{
			get {return _nodeCatcheSymbol;}
			set { _nodeCatcheSymbol = value; }
		}

		/* ... for throw Symbol event */
		private static double _nodeThrowSymbol = 2;
		public static double nodeThrowSymbol
		{
			get {return _nodeThrowSymbol;}
			set { _nodeThrowSymbol = value; }
		}

		/* ampiltude for vibrate */
		private static double halbApmtitude = 50;



		public static void vibrateY(VisualNode vn, double time)
		{
			Duration duration = new Duration(TimeSpan.FromSeconds( time ));

			// Create two DoubleAnimations and set their properties.
			// DoubleAnimation xAnimation = new DoubleAnimation();
			DoubleAnimation yAnimation = new DoubleAnimation();

			BounceEase yDirection = new BounceEase();
			yDirection.Bounces = 4;
			yDirection.Bounciness = 1.5;
			
			yAnimation.Duration = duration;
			// modify here to get the right animation
			yAnimation.EasingFunction = yDirection;

			Storyboard sb = new Storyboard();
			sb.Duration = duration;
			sb.AutoReverse = true;

			
			sb.Children.Add(yAnimation);

			Storyboard.SetTarget(yAnimation, vn.state);

			// Set the attached properties of Canvas.Left and Canvas.Top
			// to be the target properties of the two respective DoubleAnimations.
			Storyboard.SetTargetProperty(yAnimation, new PropertyPath("(Canvas.Top)"));

			/* set begin and end values */
			yAnimation.From = vn.node.y;
			yAnimation.To = vn.node.y - halbApmtitude;

			sb.Begin();
		}

		public static void rotate(VisualNode vn, double Time)
		{
			// TODO (?? Copy-Paste is OK, but it make to many objects
			// Can be optimazed ?)
		}
	}
}
