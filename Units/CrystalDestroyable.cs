using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class CrystalDestroyable : Unit
    {
        public override void TakeDamage(Tools.Color damageColor)
        {
            base.TakeDamage(damageColor);
            GameMaster.Instance.scenario.CrystalDestroyed(this);
            Destroy(gameObject);
        }

        public override bool CanBeReplaced(Unit replacingUnit, Tools.Direction direction)
        {
            return true;
        }

        public override void IsCollided(Unit collidingUnit, Tools.Direction collisionDirection)
        {
            base.IsCollided(collidingUnit, collisionDirection);
            TakeDamage(Tools.Color.None);
        }
    }
}