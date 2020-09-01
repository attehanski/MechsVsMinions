using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Glitch : DamageCard
    {
        private int slot1 = 0;
        private int slot2 = 0;

        public Glitch(int swapSlot1, int swapSlot2)
        {
            slot1 = swapSlot1;
            slot2 = swapSlot2;
            text = "Glitch " + slot1 + " - " + slot2;
        }

        public override void ExecuteCard()
        {
            GameMaster.Instance.SwapSlots(GameMaster.Instance.currentPlayer, slot1, slot2);
            base.ExecuteCard();
        }

        public override string ToString()
        {
            return "Damage: Glitch " + slot1 + "-" + slot2;
        }
    }
}