using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Ripsaw : CommandCard
    {
        public Ripsaw()
        {
            inputRequired = false;
            cardColor = Tools.Color.Blue;
            text = "Ripsaw";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_Ripsaw");
        }

        public override void InitializeCardExecution(Unit executingUnit)
        {
            base.InitializeCardExecution(executingUnit);
            cardState = CardState.NoInputRequired;
        }

        public override void ExecuteCard()
        {
            List<Unit> targets = new List<Unit>();
            for (int i = 0; i < level; i++)
            {
                MapSquare temp = startSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Forward, startFacingDirection));
                while (temp)
                {
                    if (temp.unit && !targets.Contains(temp.unit))
                    {
                        if (Tools.UnitIsEnemy(temp.unit))
                        {
                            actions.Push(new DamageAction(unit, cardColor, temp.unit));
                            targets.Add(temp.unit);
                        }
                        break;
                    }
                    temp = temp.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Forward, startFacingDirection));
                }
            }

            base.ExecuteCard();
        }
    }
}