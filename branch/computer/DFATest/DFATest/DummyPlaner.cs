using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SilverlightApplication6;

namespace DFATest
{
	class DummyPlaner<N,I> : Planner<N,I>
		where N : DummyNode
		where I : SimpleInput
	{
		public List<String> actions;
		public DummyPlaner()
		{
			actions = new List<String>();
		}
		public static string head = ">>>>>>>>>>>>>>>>>>>>";
		public static string tail = "<<<<<<<<<<<<<<<<<<<<\n";

		public void addAccept(N acceptNode)
		{
			actions.Add(head + "automate stop at "
				+ acceptNode.getNameOfNode() + " with status accept" + tail);
		}

		public void addCatchASymbol(N dest, I symbol)
		{
			actions.Add(head + dest.getNameOfNode() 
				+ " catch " 
				+ symbol.ToString() + tail);

		}

		public void addFallDownAnimation(I symbol, N des)
		{
			actions.Add(head + 
				symbol.ToString() + " falls into "
				+ des.getNameOfNode() + tail);
		}

		public void addMoveAnimation(N quellState, N destState, I symbol)
		{
			actions.Add(head + "move " + 
				symbol.ToString() + " from " + quellState +
				" to " + destState + tail);
		}

		public void addReject(N rejectNode)
		{
			actions.Add(head + "automate stop at "
				+ rejectNode.getNameOfNode() + " with status reject" + tail);
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
