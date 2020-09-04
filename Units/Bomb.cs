using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Bomb : Unit, IRepairable
    {
        public override bool CanBeReplaced(Unit replacingUnit, Tools.Direction direction)
        {
            if (replacingUnit is Minion)
                return false;
            else if (replacingUnit is Champion || replacingUnit is Bomb)
                return CanMove(direction);
            return true;
        }

        public override void TakeDamage(Tools.Color damageColor = Tools.Color.None)
        {
            base.TakeDamage(damageColor);
            Debug.Log("Bomb took damage!");
        }

        public override void Collide(Unit collisionTarget, Tools.Direction collisionDirection)
        {
            base.Collide(collisionTarget, collisionDirection);
            if (collisionTarget is Minion)
                TakeDamage();
        }

        public void RepairUnit()
        {
            Debug.Log("Repair bomb!");
        }
    }
}