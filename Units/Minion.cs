﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Minion : Unit
    {
        public override void TakeDamage(Tools.Color damageColor)
        {
            base.TakeDamage(damageColor);
            GameMaster.Instance.gearTracker.MinionKilled(this);
            Destroy(gameObject);
        }

        public override bool CanBeReplaced(Unit replacingUnit, Tools.Direction direction)
        {
            return true;
        }

        public override void Collide(Unit collidingUnit)
        {
            // Handle whatever happens to this unit
        }

        public override void IsCollided(Unit collidingUnit)
        {
            base.IsCollided(collidingUnit);
            TakeDamage(Tools.Color.None);

            // Implementation for Skewer card. Optimally should be in the Skewer script itself.
            if (GameMaster.Instance.cardBeingExecuted != null && (GameMaster.Instance.cardBeingExecuted is Skewer))
                (GameMaster.Instance.cardBeingExecuted as Skewer).hasMinionToken = true;
        }
    }
}
