using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class OilSquare : MapSquare
    {
        public override void MoveToSquare(Unit unit, Tools.Direction direction)
        {
            base.MoveToSquare(unit, direction);
            unit.Move(direction);
        }
    }
}
