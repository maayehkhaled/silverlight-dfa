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

		/* TODO: move this code to a static classe as a configuration */
		static SolidColorBrush normalState = new SolidColorBrush(Colors.Blue);
		static SolidColorBrush endState = new SolidColorBrush(Colors.Red);
		static SolidColorBrush border = new SolidColorBrush(Colors.Black);
		static SolidColorBrush edgeColor = new SolidColorBrush(Colors.Brown);
		

		public VisualNode(Node n)
		{
            outEdges = new List<Tuple<Path, VisualSymbol>>();
			node = n;
			state = new Grid();
			state.Width = node.width;
			state.Height = node.height;

			Ellipse e = new Ellipse();
			e.Width = node.width;
			e.Height = node.height;

			if (node.isEnd)
				e.Fill = endState;
			else
				e.Fill = normalState;

			e.StrokeThickness = 2;
			e.Stroke = border;

			state.Children.Add(e);

			TextBlock label = new TextBlock();
			label.Text = node.nodeLabel;
			state.Children.Add(label);
			label.TextAlignment = TextAlignment.Center;
			label.VerticalAlignment = VerticalAlignment.Center;
		}

		/*
		
		*/

		public void catchSymbol()
		{
			Animator.vibrateY(this, Animator.nodeCatcheSymbol);
		}

		public void throwSymbol()
		{
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


