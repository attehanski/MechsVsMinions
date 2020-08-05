using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class BasicMoveCard : CommandCard
    {
        public BasicMoveCard()
        {
            inputRequired = false;
        }

        public override void ExecuteCard()
        {
            CreateActions();
            base.ExecuteCard();
        }

        protected virtual void CreateActions()
        {
            MapSquare temp = startSquare.GetNeighbour(startFacing);

            for (int i = 1; i <= level; i++)
            {
                if (temp != null && temp.CanMoveToSquare(unit, startFacing))
                {
                    actions.Push(new MoveAction(unit, startFacing));
                    temp = temp.GetNeighbour(startFacing);
                }
                else
                    break;
            }
        }
    }
}