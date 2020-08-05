using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class CrystalPickable : Unit
    {
        public override void IsCollided(Unit collidingUnit)
        {
            if (collidingUnit is Champion)
            {
                collidingUnit.GetComponent<Champion>().PickUpCrystal(this);
            }
        }
    }
}