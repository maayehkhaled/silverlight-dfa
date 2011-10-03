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
		public DummyFactory(String input)
		{
			this.input = input;
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
			return new DummyNode()
		}
	}
}
