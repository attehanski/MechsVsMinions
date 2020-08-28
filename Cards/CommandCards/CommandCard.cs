using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    [System.Serializable]
    public abstract class CommandCard : Card
    {
        public Tools.Color cardColor;
        public int level = 1;

        protected Tools.Facing commandFacing;
        protected Dictionary<MapSquare, MapSquare.Interactable> inputSquares = new Dictionary<MapSquare, MapSquare.Interactable>();

        protected Stack<Action> actions = new Stack<Action>();

        // Actions are first put into a local stack and then pushed into the unit's action stack, otherwise they would be executed in reverse order.
        public override void ExecuteCard()
        {
            int actionAmnt = actions.Count;
            for (int i = 0; i < actionAmnt; i++)
            {
                unit.actionStack.Push(actions.Pop());
            }
            unit.ExecuteActions();

            base.ExecuteCard();
        }

    }
}
