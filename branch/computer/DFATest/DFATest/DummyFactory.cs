using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverlightApplication6;

namespace DFATest
{
	class DummyFactory : InputSymbolFactory<SimpleInput>
	{
		string input;
		public DummyFactory(String input)
		{
			this.input = input;
		}
		public SimpleInput create(int index)
		{
			SimpleInput s = new SimpleInput();
			String label = input[index].ToString();
			s.setLabelText(label);
			return s;
		}
	}
}
