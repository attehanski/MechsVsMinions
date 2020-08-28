using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Blaze : BasicMoveCard
    {
        private bool damageDealt = false;

        public Blaze()
        {
            cardColor = Tools.Color.Red;
            text = "Blaze";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_Blaze");
        }

        public override void InitializeCardExecution(Unit executingUnit)
        {
            base.InitializeCardExecution(executingUnit);
            damageDealt = false;
        }

        public override void Input(MapSquare squareInput)
        {
            base.Input(squareInput);
            //if (doneMoves == minMaxMoves.Item2)
            //    AddDamageActions();
        }

        public override void UpdateCardState()
        {
            if (damageDealt)
                cardState = CardState.Finished;
        }

        private void AddDamageActions()
        {
            foreach (MapSquare square in GetEndNeighbours())
                if (square.unit)
                    actions.Push(new DamageAction(unit, cardColor, square.unit));
        }

        private MapSquare[] GetEndNeighbours()
        {
            MapSquare[] endNeighbours = new MapSquare[2];
            MapSquare temp = unit.mapSquare;
            //for(int i = 0; i < actions.Count; i++)
            //{
            //    if (unit.CanMove(startFacing))
            //        temp = temp.GetNeighbour(startFacing);
            //}

            endNeighbours[0] = temp.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Left, startFacingDirection));
            endNeighbours[1] = temp.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Right, startFacingDirection));

            return endNeighbours;
        }

        public override void NoViableInputOptions()
        {
            base.NoViableInputOptions();
            Debug.Log("Blaze.NoViableInputOptions");
            AddDamageActions();

            // NOTE: This is copied from CommandCard.ExecuteCard since I couldn't find a way to call base.base.ExecuteCard or similar.
            int actionAmnt = actions.Count;
            for (int i = 0; i < actionAmnt; i++)
            {
                unit.actionStack.Push(actions.Pop());
            }
            unit.ExecuteActions();

            damageDealt = true;
        }
    }
}