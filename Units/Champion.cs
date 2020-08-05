using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Champion : Unit
    {
        public void PickUpCrystal(CrystalPickable crystal)
        {

        }

        public override bool CanMove(Tools.Direction direction)
        {
            MapSquare targetSquare = mapSquare.GetNeighbour(direction);

            // Check if target square is movable
            bool squareMovable = targetSquare != null && targetSquare.CanMoveToSquare(this, direction);
            // Check if target square has a unit that can be moved on or pushed
            bool squareIsFree = false;

            if (targetSquare.unit != null)
            {
                if (targetSquare.unit is Minion || 
                    targetSquare.unit is CrystalDestroyable || 
                    targetSquare.unit is CrystalPickable ||
                    (targetSquare.unit.canBePushed && targetSquare.unit.CanMove(direction)))
                    squareIsFree = true;
            }

            return squareMovable && squareIsFree;
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
