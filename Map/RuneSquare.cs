using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class RuneSquare : MapSquare
    {
        public Tools.Color runeColor;

        public override void MoveToSquare(Unit unit, Tools.Direction direction)
        {
            base.MoveToSquare(unit, direction);
            Debug.Log("Player stepped on rune!");
        }
    }
}