using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class BorderSquare : MapSquare
    {
        public MapSquare borderSquareCreator;

        public override bool CanEnterSquare(Unit incomingUnit, Tools.Direction moveDirection)
        {
            return false;
        }

        protected override void GenerateBorderSquares()
        {
            // Do not generate border squares for border squares
        }

        protected override void InitMaterials()
        {
            Material highlightMat = highlight.material;    // This line is for making instances of the highlight materials for testing
        }
    }
}