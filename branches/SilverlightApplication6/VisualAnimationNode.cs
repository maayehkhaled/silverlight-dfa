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

using System.Diagnostics;
using System.Windows.Markup;

namespace SilverlightApplication6
{
	public class VisualAnimationNode : VisualNode
	{
		
		public static readonly Uri COMMON_GRID_RESOURCE_URI = new Uri("VisualNodeGrid.xaml", UriKind.Relative);
		public static readonly Uri STARTEND_GRID_RESOURCE_URI = new Uri("VisualStartAndAcceptedNodeGrid.xaml", UriKind.Relative);
		public static readonly Uri START_GRID_RESOURCE_URI = new Uri("VisualStartNodeGrid.xaml", UriKind.Relative);
		public static readonly Uri END_GRID_RESOURCE_URI = new Uri("VisualAcceptNodeGrid.xaml", UriKind.Relative);
		private static string gridResourceString = null;

		private Storyboard srcAnimation;
		private Storyboard dstAnimation;
		private Storyboard acceptedAnimation;
		private Storyboard rejectedAnimation;

		

		public VisualAnimationNode(string labelText, EPoint location, bool isStartNode, bool isEndNode)
			:base(labelText, location, isStartNode, isEndNode)
		{
			// load grid as clone
			if (isStartNode && isEndNode)
			{
				Debug.WriteLine("*** is start and end node...");
				grid = getVisualNodeGrid(labelText, STARTEND_GRID_RESOURCE_URI);
			}
			else if (isStartNode)
			{
				Debug.WriteLine("*** is only start node...");
				grid = getVisualNodeGrid(labelText, START_GRID_RESOURCE_URI);
			}
			else if (isEndNode)
			{
				Debug.WriteLine("*** is only end node...");
				grid = getVisualNodeGrid(labelText, END_GRID_RESOURCE_URI);
			}
			else
			{
				Debug.WriteLine("*** is a common node...");
				grid = getVisualNodeGrid(labelText, COMMON_GRID_RESOURCE_URI);
			}

			// initialize animations
			srcAnimation = grid.Resources["srcAnimation"] as Storyboard;
			//Storyboard.SetTarget(srcAnimation, grid);
			Storyboard.SetTargetName(srcAnimation, labelText);
			//Debug.WriteLine("*** source animation children: " + srcAnimation.Children.Count);

			dstAnimation = grid.Resources["dstAnimation"] as Storyboard;
			//Storyboard.SetTarget(dstAnimation, grid);
			Storyboard.SetTargetName(dstAnimation, labelText);
			//Debug.WriteLine("*** destination animation children: " + dstAnimation.Children.Count);

			acceptedAnimation = grid.Resources["acceptedAnimation"] as Storyboard;
			rejectedAnimation = grid.Resources["rejectedAnimation"] as Storyboard;

			ellipse = grid.FindName("ellipse") as Ellipse;

			label = grid.FindName("label") as TextBlock;
			label.Text = labelText;
		}

		/* loads a new node grid by using xamlreader */
		private static Grid getVisualNodeGrid(string label, Uri type)
		{
			try
			{
				// TODO re-enable gridResourceString
				//if (gridResourceString == null)
				//{
				System.IO.Stream s = App.GetResourceStream(type).Stream;
				System.IO.StreamReader sr = new System.IO.StreamReader(s);
				gridResourceString = sr.ReadToEnd();
				//}
				//Debug.WriteLine(gridResourceString);

				Grid grid = (Grid)XamlReader.Load(gridResourceString);
				grid.Name = label;
				//grid.RenderTransform = new CompositeTransform();

				return grid;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		/* returns the storyboard for the source animation */
		public Storyboard getSrcAnimation()
		{
			return srcAnimation;
		}

		/* returns the storyboard for the destination animation */
		public Storyboard getDstAnimation()
		{
			return dstAnimation;
		}

		/* returns the storyboard for the accepted animation */
		public Storyboard getAcceptedAnimation()
		{
			return acceptedAnimation;
		}

		/* returns the storyboard for the rejected animation */
		public Storyboard getRejectedAnimation()
		{
			return rejectedAnimation;
		}

		
	}
}
