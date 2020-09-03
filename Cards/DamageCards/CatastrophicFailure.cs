using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class CatastrophicFailure : DamageCard
    {
        public CatastrophicFailure()
        {
            slottable = false;
            text = "Take 2 dmg";
        }

        public override void ExecuteCard()
        {
            GameMaster.Instance.currentPlayer.character.TakeDamage();
            GameMaster.Instance.currentPlayer.character.TakeDamage();
            base.ExecuteCard();
        }

        public override string ToString()
        {
            return "Damage: Catastrophic Failure";
        }
    }
}