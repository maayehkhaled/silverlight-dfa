
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
	/*create a instance of VisualAnimatinInput */
	public class AnimationInputFactory<N,I> : AnimationFactory<N, I>
		where N : VisualAnimationNode
		where I : VisualAnimationInput
	{
		private List<I> inputChars ;
		private List<N> nodes;

		public I createInputAnimation(int indexOfInputChar)
		{
			return inputChars[indexOfInputChar];
		}

		public void setInputChars(List<I> input)
		{
			inputChars = input;
		}

		public void setNodes(List<N> nodes)
		{
			this.nodes = nodes;
		}

		public N createVisualNode(string label)
		{
			foreach (N n in nodes)
			{
				if (label.Trim().Equals(n.getLabelText()))
				{
					return n;
				}
			}
			return null;
		}

	}
}
