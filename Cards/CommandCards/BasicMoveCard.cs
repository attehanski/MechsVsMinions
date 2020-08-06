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
            for (int i = 1; i <= level; i++)
            {
                unit.Move(startFacing);
            }
        }
    }
}