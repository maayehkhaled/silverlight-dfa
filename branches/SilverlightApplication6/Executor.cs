

using System;
using System.Diagnostics;

using SCG = System.Collections.Generic;
//using C5;

using state = System.String;
using input = System.Char;
using System.IO;
using System.Collections.Generic;



namespace SilverlightApplication6
{
	/// <summary>
	/// Implements a deterministic finite automata (DFA)
	/// </summary>
	class DFA<N, I>
		where N : VisualNode
		where I : VisualInput
	{
		// Start state
		public state start;
		// Set of final states
		public SCG.HashSet<state> final;
		// Transition table
		public SCG.IDictionary<SCG.KeyValuePair<state, input>, state> transTable;



		public DFA(List<VisualNode> visualNodes)
		{
			final = new SCG.HashSet<state>();
			transTable = new SCG.Dictionary<SCG.KeyValuePair<state, input>, state>();
			// initial set of final states and initial the transitionTable
			foreach (VisualNode s in visualNodes)
			{
				if (s.isEndNode)
				{
					Debug.WriteLine(">>>>>>>>>>>>>>>>>>add "
						+ s.ToString()
						+ " to final set<<<<<<<<<<<<<<<<");
					final.Add(s.getLabelText());
				}
				foreach (Tuple<VisualNode, string> a in s.adjacenceList)
				{
					if (!a.Item2.Contains("|"))
					{
						transTable.Add(new SCG.KeyValuePair<state, input>(
							s.getLabelText(), a.Item2.ToCharArray()[0]), a.Item1.getLabelText());
						Debug.WriteLine(">>>>>>>>>>>>>>>>>>("
							+ s.ToString() + " " + a.Item2 + ") -> " + a.Item1.ToString() );

					}
					else
					{
						string[] symbols = a.Item2.Trim().Split('|');
						foreach (string m in symbols)
						{
							transTable.Add(new SCG.KeyValuePair<state, input>(
								s.getLabelText(), m.ToCharArray()[0]), a.Item1.getLabelText());
							Debug.WriteLine(">>>>>>>>>>>>>>>>>>("
							+ s.ToString() + " " + m + ") -> " + a.Item1.ToString());

						}
					}

				}
			}
			// initial the start state
			foreach (VisualNode s in visualNodes)
			{
				if (s.isStartNode)
				{
					start = s.getLabelText();
					break;
				}
			}
		}

		public string Simulate(string inputString,
			Planner<N, I> plan,
			AnimationFactory<N, I> factory)
		{
			/* initital */
			state curState = start;
			plan.clean();

			/* convert inputString to array of chars */
			char[] input = new char[inputString.Length]; //inputString.ToCharArray();
			StringReader sr = new StringReader(inputString);
			sr.Read(input, 0, inputString.Length);

			for (int i = 0; i < input.Length; ++i)
			{
				SCG.KeyValuePair<state, input> transition =
					new SCG.KeyValuePair<state, input>(curState, input[i]);
				/*-- visual --*/
				I s = factory.createInputAnimation(i);
				N currentVisualState = factory.createVisualNode(curState);
				plan.addFallDownAnimation(s, currentVisualState);
				plan.addCatchASymbol(currentVisualState, s);
				/*++ ++*/

				if (!transTable.ContainsKey(transition))
				{
					/*-- --*/
					plan.addReject(currentVisualState);
					/*++ ++*/
					return "Rejected";
				}

				curState = transTable[transition];
				/*-- --*/
				N old = currentVisualState;
				currentVisualState = factory.createVisualNode(curState);
				plan.addMoveAnimation(old, currentVisualState, s);
				/*++ ++*/
			}

			if (final.Contains(curState))
			{
				/*-- --*/
				N currentVisualState = factory.createVisualNode(curState);
				plan.addAccept(currentVisualState);
				/*++ ++*/
				return "Accepted";
			}
			else
			{
				/*-- --*/
				N currentVisualState = factory.createVisualNode(curState);
				plan.addReject(currentVisualState);
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
