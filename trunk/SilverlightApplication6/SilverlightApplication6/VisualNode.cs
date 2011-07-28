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

namespace SilverlightApplication6
{
	public class VisualNode
	{
		public readonly Node node;
		public Grid state;
		public List<Tuple<Path,VisualSymbol> > outEdges;
        private IDictionary<string, VisualNode> followers = new Dictionary<string, VisualNode>();
        public Ellipse e;

		public VisualNode(Node n)
		{
            outEdges = new List<Tuple<Path, VisualSymbol>>();
			node = n;
			state = new Grid();
			state.Width = node.width;
			state.Height = node.height;

            e = new Ellipse();
			e.Width = node.width;
			e.Height = node.height;

            if (node.isStart && node.isEnd)
            {
                e.Fill = (LinearGradientBrush)Application.Current.Resources["startEndStateBrush"];
            }
            else if (node.isStart)
            {
                e.Fill = (LinearGradientBrush)Application.Current.Resources["startStateBrush"];
            }
            else if (node.isEnd)
            {
                e.Fill = (LinearGradientBrush)Application.Current.Resources["endStateBrush"];
            }
            else
            {
                e.Fill = (LinearGradientBrush)Application.Current.Resources["normalStateBrush"];
            }

            e.StrokeThickness = 1;
            e.Stroke = (LinearGradientBrush)Application.Current.Resources["stateBorderBrush"];

			state.Children.Add(e);

			TextBlock label = new TextBlock();
			label.Text = node.nodeLabel;
			state.Children.Add(label);
			label.TextAlignment = TextAlignment.Center;
			label.VerticalAlignment = VerticalAlignment.Center;
		}

        public void addFollower(string symbol, VisualNode node)
        {
            followers.Add(symbol, node);
        }

        public bool TryGetFollower(string symbol, out VisualNode node)
        {
            return followers.TryGetValue(symbol, out node);
        }
	}

	/** presents the label of the egde (input symbol)*/
	public class VisualSymbol
	{
		static SolidColorBrush textColor = new SolidColorBrush(Colors.Blue);

		private EPoint _coordinate;
		public EPoint coordinate
		{
			get { return _coordinate; }
			set { _coordinate = value; }
		}

		private TextBlock _label;
		public TextBlock label 
		{
			get {return _label; }  
			set {_label = value; } 
		}
		
		public VisualSymbol(string edgeLabel, EPoint coordinate)
		{
			this.coordinate = coordinate;
			this.label = new TextBlock();
			label.Text = edgeLabel;
		}

	}
}


