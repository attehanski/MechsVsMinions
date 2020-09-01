using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class MajorGlitch : DamageCard
    {
        public override void ExecuteCard()
        {
            GameMaster.Instance.SwapSlots(GameMaster.Instance.currentPlayer, 0, 5);
            GameMaster.Instance.SwapSlots(GameMaster.Instance.currentPlayer, 1, 4);
            GameMaster.Instance.SwapSlots(GameMaster.Instance.currentPlayer, 2, 3);
            base.ExecuteCard();
        }

        public override string ToString()
        {
            return "Damage: Major Glitch";
        }
    }
}