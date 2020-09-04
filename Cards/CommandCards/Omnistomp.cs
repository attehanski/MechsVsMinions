using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Omnistomp : BasicMoveCard
    {
        private Dictionary<MapSquare, Tools.Facing> inputOptionFacings = new Dictionary<MapSquare, Tools.Facing>();

        public Omnistomp()
        {
            cardColor = Tools.Color.Green;
            inputRequired = true;
            text = "Omnistomp";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_Omnistomp");
        }

        public override Dictionary<MapSquare, MapSquare.Interactable> GetValidInputSquares()
        {
            inputSquares.Clear();
            inputOptionFacings.Clear();

            CheckTowingStatus();

            Tools.Facing[] possibleFacings;
            if (doneMoves == 0)
                possibleFacings = new Tools.Facing[] { Tools.Facing.Left, Tools.Facing.Right, Tools.Facing.Forward };
            else
                possibleFacings = new Tools.Facing[] { moveFacing };

            foreach (Tools.Facing facing in possibleFacings)
            {
                Tools.Direction moveDir = Tools.GetDirectionFromFacing(facing, startFacingDirection);
                foreach (KeyValuePair<MapSquare, MapSquare.Interactable> kvp in GetInputsForOneDirection(moveDir))
                {
                    inputSquares.Add(kvp.Key, kvp.Value);
                    inputOptionFacings.Add(kvp.Key, facing);
                }
            }

            return inputSquares;
        }

        public override void Input(MapSquare inputSquare)
        {
            moveFacing = inputOptionFacings[inputSquare];
            base.Input(inputSquare);
        }
    }
}