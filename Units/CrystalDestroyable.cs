﻿using System.Collections;
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

        public override void IsCollided(Unit collidingUnit)
        {
            base.IsCollided(collidingUnit);
            TakeDamage(Tools.Color.None);
        }
    }
}