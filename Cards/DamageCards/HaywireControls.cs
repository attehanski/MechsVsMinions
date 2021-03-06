﻿using System.Collections;
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
            text = "Move 2 rnd";
        }

        public override void ExecuteCard()
        {
            damageDir = GameMaster.Instance.GetRandomDirection();
            for (int i = 0; i < 2; i++)
            {
                GameMaster.Instance.currentPlayer.character.Move(damageDir);
            }
            GameMaster.Instance.currentPlayer.character.ExecuteActions();
            base.ExecuteCard();
        }

        public override string ToString()
        {
            return "Damage: Haywire Controls";
        }
    }
}