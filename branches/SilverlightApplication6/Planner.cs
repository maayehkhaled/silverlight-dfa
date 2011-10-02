using System;
namespace SilverlightApplication6
{
	public interface Planner
	{
		void addAccept(VisualNode acceptNode);
		void addCatchASymbol(VisualNode dest, VisualAnimationInput symbol);
		void addFallDownAnimation(VisualAnimationInput symbol, VisualNode des);
		void addMoveAnimation(VisualNode quellState, VisualNode destState, VisualAnimationInput symbol);
		void addReject(VisualNode rejectNode);
		void clean();
	}
}
