using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class SchoolSquare : MapSquare
    {
        public bool damageMinions = true;

        public override void MoveToSquare(Unit movingUnit, Tools.Direction direction)
        {
            if (movingUnit is Minion && damageMinions)
            {
                movingUnit.TakeDamage();
                GameMaster.Instance.scenario.MinionReachedSchool();
            }
            if (unit)
                movingUnit.Collide(unit);

            movingUnit.ChangeSquare(this); // NOTE: This probably needs to change for cases where we can't take up this space. Needs to be checked.
        }
    }
}