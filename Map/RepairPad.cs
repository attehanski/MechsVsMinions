using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class RepairPad : MapSquare
    {
        public override void MoveToSquare(Unit unit, Tools.Direction direction)
        {
            base.MoveToSquare(unit, direction);
            if (unit is IRepairable)
                (unit as IRepairable).RepairUnit();
        }
    }
}