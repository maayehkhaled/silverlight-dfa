using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverlightApplication6;

namespace DFATest
{
	class SimpleInput : SilverlightApplication6.VisualInput
	{
		string label;
		public string getLabelText()
		{
			return label;
		}
		public SimpleInput()
		{
			label = "hello";
		}
		public void setLabelText(string labelText)
		{
			label = labelText;
		}

		override public string ToString()
		{
			return label;
		}
	}
}
