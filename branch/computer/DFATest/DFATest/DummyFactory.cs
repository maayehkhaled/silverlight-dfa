using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverlightApplication6;

namespace DFATest
{
	class DummyFactory<N,T> : AnimationFactory<N,T>
		where T : SimpleInput
		where N : DummyNode
	{
		string input;
		List<N> lstOfNode;
		public DummyFactory(String input)
		{
			this.input = input;
			lstOfNode = new List<N>();
		}
		public void setNode(List<N> nodes)
		{
			lstOfNode = nodes;
		}

		public T createInputAnimation(int index)
		{
			T s = (T)new SimpleInput ();
			String label = input[index].ToString();
			s.setLabelText(label);
			return s;
		}
		
		public N createVisualNode(string nodeTextLabel)
		{
			foreach(N n in lstOfNode)
			{
				if (nodeTextLabel.Trim().Equals(n.getLabelText()) )
				{
					return n;
				}
			}
			return null;
		}
	}
}
