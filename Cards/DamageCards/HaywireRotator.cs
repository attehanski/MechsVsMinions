using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class HaywireRotator : DamageCard
    {
        private Tools.Direction damageDir;

        public HaywireRotator()
        {
            slottable = true;
        }

        public override void ExecuteCard()
        {
            damageDir = GameMaster.Instance.GetRandomDirection();
            GameMaster.Instance.currentPlayer.character.actionStack.Push(new TurnAction(GameMaster.Instance.currentPlayer.character, damageDir));
            GameMaster.Instance.currentPlayer.character.ExecuteActions();
        }

        public override string ToString()
        {
            return "Damage: Haywire Rotator";
        }
    }
}