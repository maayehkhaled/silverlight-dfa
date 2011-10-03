using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SilverlightApplication6;

namespace DFATest
{
	class DummyPlaner : Planner<VisualNode,SimpleInput>
	{
		public List<String> actions;
		public DummyPlaner()
		{
			actions = new List<String>();
		}
		public static string head = ">>>>>>>>>>>>>>>>>>>>";
		public static string tail = "<<<<<<<<<<<<<<<<<<<<\n";

		public void addAccept(VisualNode acceptNode)
		{
			actions.Add(head + "automate stop at "
				+ acceptNode.ToString() + " with status accept" + tail);
		}

		public void addCatchASymbol(VisualNode dest, SimpleInput symbol)
		{
			actions.Add(head + dest.ToString() 
				+ " catch " 
				+ ((SimpleInput)symbol).ToString() + tail);

		}

		public void addFallDownAnimation(SimpleInput symbol, VisualNode des)
		{
			actions.Add(head + 
				((SimpleInput)symbol).ToString() + " falls into "
				+ des.ToString() + tail);
		}

		public void addMoveAnimation(VisualNode quellState, VisualNode destState, SimpleInput symbol)
		{
			actions.Add(head + "move " + 
				((SimpleInput)symbol).ToString() + " from " + quellState +
				" to " + destState + tail);
		}

		public void addReject(VisualNode rejectNode)
		{
			actions.Add(head + "automate stop at "
				+ rejectNode.ToString() + " with status reject" + tail);
		}

		public void clean()
		{
			actions.Clear();
		}

		public List<String> getAction()
		{
			return actions;
		}
	}
}
