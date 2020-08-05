using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Blaze : BasicMoveCard
    {
        public Blaze()
        {
            cardColor = Tools.Color.Red;
            inputRequired = false;
            text = "Blaze";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_Blaze");
        }

        protected override void CreateActions()
        {
            // NOTE: This adds the actions in wrong order, with the damage being in correct place but at start of stack!
            base.CreateActions();
            foreach (MapSquare square in GetEndNeighbours())
                if (square.unit)
                    actions.Push(new DamageAction(unit, cardColor, square.unit));
        }

        private MapSquare[] GetEndNeighbours()
        {
            MapSquare[] endNeighbours = new MapSquare[2];
            MapSquare temp = startSquare;
            for(int i = 0; i < actions.Count; i++) // Simulate movement until end, 
            {
                temp = temp.GetNeighbour(startFacing);
            }

            endNeighbours[0] = temp.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Left, startFacing));
            endNeighbours[1] = temp.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Right, startFacing));

            return endNeighbours;
        }
    }
}