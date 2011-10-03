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
		private List<VisualAnimationInput> inputChars ;
		 
		public VisualAnimationInput createInputChar(int indexOfInputChar)
		{
			return inputChars[indexOfInputChar];
		}

		public void setInputChars(List<VisualAnimationInput> input)
		{
			inputChars = input;
		}
	}
}
