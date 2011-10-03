using System;
namespace SilverlightApplication6
{
	public interface Planner<N,I>
		where N: VisualNode 
		where I: VisualInput
	{
		void addAccept(N acceptNode);
		void addCatchASymbol(N dest, I symbol);
		void addFallDownAnimation(I symbol, N des);
		void addMoveAnimation(N quellState, N destState, I symbol);
		void addReject(N rejectNode);
		void clean();
	}
}
