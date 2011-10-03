

using System;
using System.Diagnostics;

using SCG = System.Collections.Generic;
//using C5;

using state = SilverlightApplication6.VisualNode;
using input = System.Char;
using System.IO;
using System.Collections.Generic;
using DFATest;


namespace SilverlightApplication6
{
	/// <summary>
	/// Implements a deterministic finite automata (DFA)
	/// </summary>
	class DFA<P,F,I>  
		where P : Planner<VisualNode,I>
		where I : VisualInput
		where F: InputSymbolFactory<I>
	{
		// Start state
		public state start;
		// Set of final states
		public SCG.HashSet<state> final;
		// Transition table
		public SCG.IDictionary<SCG.KeyValuePair<state, input>, state> transTable;

		public P plan;

		public DFA(List<state> visualNodes, P plan)
		{
			this.plan = plan;
			final = new SCG.HashSet<state>();
			transTable = new SCG.Dictionary<SCG.KeyValuePair<state, input>, state>();
			// initial set of final states and initial the transitionTable
			foreach (state s in visualNodes)
			{
				if (s.isEndNode)
				{
					Debug.Write(">>>>>>>>>>>>>>>>>>add " 
						+ s.ToString() 
						+ " to final set<<<<<<<<<<<<<<<<\n");
					final.Add(s);
				}
				foreach (Tuple<state, string> a in s.adjacenceList)
				{
					if (!a.Item2.Contains("|"))
					{
						transTable.Add(new SCG.KeyValuePair<state, input>(
							s, a.Item2.ToCharArray()[0]), a.Item1);
						Debug.Write(">>>>>>>>>>>>>>>>>>("
							+ s.ToString() + " " + a.Item2 + ") -> " + a.Item1.ToString() + "\n");
						
					}
					else
					{
						string[] symbols = a.Item2.Trim().Split('|');
						foreach (string m in symbols)
						{
							transTable.Add(new SCG.KeyValuePair<state, input>(
								s, m.ToCharArray()[0]), a.Item1);
							Debug.Write(">>>>>>>>>>>>>>>>>>("
							+ s.ToString() + " " + m + ") -> " + a.Item1.ToString() + "\n");
							
						}
					}

				}
			}
			// initial the start state
			foreach (state s in visualNodes)
			{
				if (s.isStartNode)
				{
					start = s;
					break;
				}
			}
		}

		public string Simulate(string inputString, F factory)
		{
			/* initital */
			state curState = start;
			plan.clean();

			/* convert inputString to array of chars */
			char[] input = new char[inputString.Length]; //inputString.ToCharArray();
			StringReader sr = new StringReader(inputString);
			sr.Read(input, 0, inputString.Length);
			
			for (int i = 0; i < input.Length; ++i )
			{
				SCG.KeyValuePair<state, input> transition =
					new SCG.KeyValuePair<state, input>(curState, input[i]);
				/*-- visual --*/
				I s = factory.create(i);
				plan.addFallDownAnimation(s, curState);
				plan.addCatchASymbol(curState, s);
				/*++ ++*/

				if (!transTable.ContainsKey(transition))
				{
					/*-- --*/
					plan.addReject(curState);
					/*++ ++*/
					return "Rejected";
				}
				state old = curState;
				curState = transTable[transition];
				/*-- --*/
				plan.addMoveAnimation(old, curState, s);
				/*++ ++*/
			}

			if (final.Contains(curState))
			{
				/*-- --*/
				plan.addAccept(curState);
				/*++ ++*/
				return "Accepted";
			}else
			{
				/*-- --*/
				plan.addReject(curState);
				/*++ ++*/
				return "Rejected";
			}
		}

	}
	/*
	/// <summary>
	/// Implements a comparer that suits the transTable SordedList
	/// </summary>
	public class Comparer : SCG.IComparer<SCG.KeyValuePair<state, input>>
	{
		public int Compare(SCG.KeyValuePair<state, input> transition1,
			SCG.KeyValuePair<state, input> transition2)
		{
			if (transition1.Key == transition2.Key)
				return transition1.Value.CompareTo(transition2.Value);
			else
				return transition1.Key.CompareTo(transition2.Key);
		}
	}
	 * 
	 */

}
