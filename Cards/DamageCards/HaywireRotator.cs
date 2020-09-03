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
            text = "Turn rnd direction";
        }

        public override void ExecuteCard()
        {
            damageDir = GameMaster.Instance.GetRandomDirection();
            GameMaster.Instance.currentPlayer.character.Turn(damageDir);
            GameMaster.Instance.currentPlayer.character.ExecuteActions();
            base.ExecuteCard();
        }

        public override string ToString()
        {
            return "Damage: Haywire Rotator";
        }
    }
}