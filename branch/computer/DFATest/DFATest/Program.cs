using System;

using SCG = System.Collections.Generic;
using System.Linq;
using System.Text;

using SilverlightApplication6;

using state = System.Int32;
using input = System.Char;
using System.Collections.Generic;
using System.Diagnostics;
namespace DFATest
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write("test DFA\n");

			List<VisualNode> visualNodes = new List<VisualNode>();
			VisualNode q1 = new VisualNode("q1", new EPoint(), true, false);
			VisualNode q2 = new VisualNode("q2", new EPoint(), false, true);
			VisualNode q3 = new VisualNode("q3", new EPoint(), false, false);

			q1.addAdjacenceList(q1, "b");
			q1.addAdjacenceList(q2, "a");

			q2.addAdjacenceList(q3, "a");
			q2.addAdjacenceList(q3, "b");

			q3.addAdjacenceList(q1, "b");

			visualNodes.Add(q1);
			visualNodes.Add(q2);
			visualNodes.Add(q3);

			var planner = new DummyPlaner<DummyNode, SimpleInput>();
			var sipserDFA = new DFA<DummyNode,SimpleInput>(visualNodes);
			
			String input = "aababba";
			DummyFactory<DummyNode, SimpleInput> dfactory = 
				new DummyFactory<DummyNode,SimpleInput>(input);
			List<DummyNode> representer = new List<DummyNode>();
			
			foreach (VisualNode x in visualNodes)
			{
				DummyNode d = new DummyNode(x.getLabelText(), new EPoint(), x.isStartNode, x.isEndNode);
				representer.Add(d);
			}
			dfactory.setNode(representer);

			String result  = sipserDFA.Simulate(input, planner, dfactory);
			Debug.WriteLine(result);
			foreach (String action in planner.getAction())
			{
				Debug.Write(action);
			}
			
		}
	}
}
