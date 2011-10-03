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
	public  interface AnimationFactory<N,I>  where I: VisualInput where N:VisualNode
	{
		/* indexOfInputChar: begining with 0 */
		I createInputAnimation(int indexOfInputChar);
		N createVisualNode(string nodeTextLabel);
	}
}
