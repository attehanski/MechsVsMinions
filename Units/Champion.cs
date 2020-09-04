using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Champion : Unit, IRepairable
    {
        public Champion()
        {
            towable = true;
        }

        public void PickUpCrystal(CrystalPickable crystal)
        {

        }

        public override bool CanBeReplaced(Unit replacingUnit, Tools.Direction direction)
        {
            if (replacingUnit is Minion)
                return false;
            else if (replacingUnit is Champion || replacingUnit is Bomb)
                return CanMove(direction);
            return true;
        }

        public override void Collide(Unit collisionTarget, Tools.Direction collisionDirection)
        {
            base.Collide(collisionTarget, collisionDirection);
        }

        public override void TakeDamage(Tools.Color damageColor = Tools.Color.None)
        {
            DamageCard damage = GameMaster.Instance.damageCardDeck.DrawCard();
            Debug.Log("Champion taking damage! " + damage.ToString());
            damage.ExecuteCard();
            if (damage.slottable) damage.SlotDamageCard();
            GameMaster.Instance.damageCardDeck.DiscardCard(damage);
            base.TakeDamage(damageColor);
        }

        public void RepairUnit()
        {
            Debug.Log("Repair player character!");
        }
    }
}
