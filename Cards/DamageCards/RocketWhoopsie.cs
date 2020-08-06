using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class RocketWhoopsie : DamageCard
    {
        public RocketWhoopsie()
        {
            slottable = true;
        }

        public override void ExecuteCard()
        {
            for (int i = 0; i < 3; i++)
            {
                GameMaster.Instance.currentPlayer.character.Move(Tools.Facing.Forward);
            }

            GameMaster.Instance.currentPlayer.character.Turn(Tools.Facing.Back);

            GameMaster.Instance.currentPlayer.character.ExecuteActions();
        }

        public override string ToString()
        {
            return "Damage: Rocket Whoopsie";
        }
    }
}