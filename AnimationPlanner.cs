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
using System.Diagnostics;
using System.Collections.Generic;

namespace SilverlightApplication6
{
    // static class for planning the animation sequence

	public class AnimationPlanner<N,I> : Planner<N,I>
		where N: VisualAnimationNode
		where I: VisualAnimationInput
    {
		 
        
		List<Storyboard> animations;
		List<String> actions;

		public static string head = ">>>>>>>>>>>>>>>>>>";
		public static string tail = "<<<<<<<<<<<<<<<<<<";

		public AnimationPlanner()
		{
			animations = new List<Storyboard>();
			actions = new List<string>();
		}

		/* 
		 * add an animation simulating a symbols fall down from overleft coner to 
		 * dest. State 
		 */
		public void addFallDownAnimation(I symbol, N des)
		{
			symbol.setConsumeAnimationTo(des.location);
			animations.Add(symbol.getConsumeAnimation());
			animations.Add(symbol.getSearchAnimation());

			actions.Add(head + "Symbol " + symbol.getLabelText()
							+ " falls into " + des.ToString() + tail);
		}

		/*
		 * add an animation simulating a symbol move from a state to a new state 
		 */
		public void addMoveAnimation(N quellState, N destState, I symbol)
		{
			animations.Add(symbol.getFadeInAnimation());
			actions.Add(head + "move " + symbol.getLabelText() +
				" from " + quellState.ToString() + " to " + destState.ToString() + tail);
		}

		/*
		 * add an animation simulating state catch a symbol 
		 */
		public void addCatchASymbol(N dest, I symbol)
		{
			animations.Add(dest.getDstAnimation());

			actions.Add(head + "state " + dest.ToString()
				+ " catches symbol " + symbol.getLabelText() + tail);
		}

		/*
		 * add an animation simulation state accept input
		 */
		public void addAccept(N acceptNode)
		{
			animations.Add(acceptNode.getAcceptedAnimation());
			actions.Add(head  + "DFA stops at " + acceptNode.ToString() 
				+ " with status accept"+ tail);
		}

		public void addReject(N rejectNode)
		{
			animations.Add(rejectNode.getRejectedAnimation());
			actions.Add(head + "DFA stops at " + rejectNode.ToString()
				+ " with status reject" + tail);
		}

		public void clean()
		{
			animations.Clear();
			actions.Clear();
		}

		public void showActionInDebug()
		{
			foreach(string s in actions)
			{
				Debug.WriteLine(head + s + tail);
			}
		}

		public List<Storyboard> getAnimations()
		{
			return this.animations;
		}
	}
	


}
