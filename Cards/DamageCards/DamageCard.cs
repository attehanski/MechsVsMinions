﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    [System.Serializable]
    public abstract class DamageCard : Card
    {
        public bool slottable = false;

        public DamageCard()
        {
            type = Type.Damage;
            text = ToString();
            textureAsset = Resources.Load<Sprite>("T_DamageCard_Generic"); // For testing before cards have specific textures
        }

        public override void ExecuteCard()
        {
            
        }

        public virtual void SlotDamageCard()
        {
            int slot = GameMaster.Instance.GetDiceRoll();
            Debug.Log("Slotting Damage " + text + " to slot " + slot);
            GameMaster.Instance.SlotCard(UIMaster.Instance.cardSlots[slot], this);
        }

        public override void InitializeCardExecution(Unit unit)
        {
            base.InitializeCardExecution(unit);
            cardState = CardState.NoInputRequired;
        }
    }
}