using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

//using System;
using SCG = System.Collections.Generic;
//using C5;

//using state = System.Int32;
//using input = System.Char;

namespace SilverlightApplication6
{
	/// <summary>
	/// Implements a deterministic finite automata (DFA)
	/// </summary>
	class DFA
	{
		/*
		// Start state
		public state start;
		// Set of final states
		public Set<state> final;
		// Transition table
		public SCG.SortedList<KeyValuePair<state, input>, state> transTable;

		public DFA()
		{
			final = new Set<state>();

			transTable = new SCG.SortedList<KeyValuePair<state, input>, state>(new Comparer());
		}

		public string Simulate(string @in)
		{
			state curState = start;

			CharEnumerator i = @in.GetEnumerator();

			while (i.MoveNext())
			{
				KeyValuePair<state, input> transition = new KeyValuePair<state, input>(curState, i.Current);

				if (!transTable.ContainsKey(transition))
					return "Rejected";

				curState = transTable[transition];
			}

			if (final.Contains(curState))
				return "Accepted";
			else
				return "Rejected";
		}

		public void Show()
		{
			Console.Write("DFA start state: {0}\n", start);
			Console.Write("DFA final state(s): ");

			SCG.IEnumerator<state> iE = final.GetEnumerator();

			while (iE.MoveNext())
				Console.Write(iE.Current + " ");

			Console.Write("\n\n");

			foreach (SCG.KeyValuePair<KeyValuePair<state, input>, state> kvp in transTable)
				Console.Write("Trans[{0}, {1}] = {2}\n", kvp.Key.Key, kvp.Key.Value, kvp.Value);
		}
	}

	/// <summary>
	/// Implements a comparer that suits the transTable SordedList
	/// </summary>
	public class Comparer : SCG.IComparer<KeyValuePair<state, input>>
	{
		public int Compare(KeyValuePair<state, input> transition1, KeyValuePair<state, input> transition2)
		{
			if (transition1.Key == transition2.Key)
				return transition1.Value.CompareTo(transition2.Value);
			else
				return transition1.Key.CompareTo(transition2.Key);
		}
	
	*/ }

}