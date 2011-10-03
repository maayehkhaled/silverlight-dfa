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

			var planer = new DummyPlaner<DummyNode, SimpleInput>();
			var sipserDFA = new DFA<DummyNode,SimpleInput>(visualNodes, planer);
			
			String input = "aababba";
			DummyFactory<SimpleInput> dfactory = new DummyFactory<SimpleInput>(input);
			String result  = sipserDFA.Simulate(input, dfactory);
			Debug.WriteLine(result);
			foreach (String action in planer.getAction())
			{
				Debug.Write(action);
			}
			
		}
	}
}
