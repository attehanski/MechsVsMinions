using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class CrystalPickable : Unit
    {
        public override bool CanBeReplaced(Unit replacingUnit, Tools.Direction direction)
        {
            return true;
        }

        public override void IsCollided(Unit collidingUnit, Tools.Direction collisionDirection)
        {
            if (collidingUnit is Champion)
            {
                collidingUnit.GetComponent<Champion>().PickUpCrystal(this);
            }
        }
    }
}