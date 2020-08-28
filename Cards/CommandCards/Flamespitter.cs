using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Flamespitter : CommandCard
    {
        public Flamespitter()
        {
            inputRequired = false;
            cardColor = Tools.Color.Red;
            text = "Flamespitter";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_Flamespitter");
        }

        public override void InitializeCardExecution(Unit executingUnit)
        {
            base.InitializeCardExecution(executingUnit);
            cardState = CardState.NoInputRequired;
        }

        public override void ExecuteCard()
        {
            foreach (MapSquare targetSquare in GetTargetSquares())
                if (targetSquare.unit && Tools.UnitIsEnemy(targetSquare.unit))
                    actions.Push(new DamageAction(unit, cardColor, targetSquare.unit));
            base.ExecuteCard();
        }

        private Queue<MapSquare> GetTargetSquares()
        {
            Queue<MapSquare> targetSquares = new Queue<MapSquare>();
            MapSquare temp = startSquare;
            for (int i = 0; i < 2; i++)
            {
                temp = temp.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Forward, startFacingDirection));
                if (temp)
                    targetSquares.Enqueue(temp);
            }
            for (int i = 1; i < level; i++)
            {
                if (temp)
                {
                    if (!targetSquares.Contains(temp))
                        targetSquares.Enqueue(temp);
                    if (temp.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Right, startFacingDirection)))
                        targetSquares.Enqueue(temp.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Right, startFacingDirection)));
                    if (temp.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Left, startFacingDirection)))
                        targetSquares.Enqueue(temp.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Left, startFacingDirection)));

                temp = temp.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Forward, startFacingDirection));
                }
            }

            return targetSquares;
        }
    }
}