using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class DamageAction : Action
    {
        public Tools.Color damageColor;
        public Unit target;

        private bool damageDealt = false;

        public DamageAction(Unit dealingUnit, Tools.Color dmgColor, Unit receivingUnit)
        {
            unit = dealingUnit;
            damageColor = dmgColor;
            target = receivingUnit;
        }

        public override void UpdateState()
        {
            if (!damageDealt)
            {
                target.TakeDamage(damageColor);
                damageDealt = true;
                actionFinished = true;
            }

            //if (!actionInProgress)
            //{
            //    actionInProgress = true;
            //}

            //if (actionInProgress)
            //{

            //}
        }
    }
}