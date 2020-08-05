using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class HaywireControls : DamageCard
    {
        private Tools.Direction damageDir;

        public HaywireControls()
        {
            slottable = true;
        }

        public override void ExecuteCard()
        {
            damageDir = GameMaster.Instance.GetRandomDirection();
            for (int i = 0; i < 2; i++)
            {
                GameMaster.Instance.currentPlayer.character.actionStack.Push(new MoveAction(GameMaster.Instance.currentPlayer.character, damageDir));
            }
            GameMaster.Instance.currentPlayer.character.ExecuteActions();
        }

        public override string ToString()
        {
            return "Damage: Haywire Controls";
        }
    }
}