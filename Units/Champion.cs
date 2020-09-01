using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Champion : Unit
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
            else if (replacingUnit is Champion /*|| replacingUnit is Bomb*/)
                return CanMove(direction);
            return true;
        }

        public override void Collide(Unit collisionTarget)
        {
            base.Collide(collisionTarget);
        }

        public override void TakeDamage(Tools.Color damageColor = Tools.Color.None)
        {
            DamageCard damage = GameMaster.Instance.DrawCard(Card.Type.Damage) as DamageCard;
            Debug.Log("Champion taking damage! " + damage.ToString());
            damage.ExecuteCard();
            if (damage.slottable) damage.SlotDamageCard();
            GameMaster.Instance.DiscardCard(damage);
            base.TakeDamage(damageColor);
        }

    }
}
