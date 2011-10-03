using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverlightApplication6;

namespace DFATest
{
	class DummyNode : VisualNode
	{

		public DummyNode(string labelText, EPoint location, bool isStartNode, bool isEndNode)
			:base(labelText,location,isStartNode,isEndNode)
		{
		}

		public override string ToString()
		{
			return labelText;
		}

		public string getNameOfNode()
		{
			return labelText.ToUpper();
		}
	}

}
