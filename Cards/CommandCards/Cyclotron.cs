using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Cyclotron : BasicTurnCard
    {
        public Cyclotron()
        {
            cardColor = Tools.Color.Yellow;
            inputRequired = true;
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_Cyclotron");
            text = "Cyclotron";
        }

        public override void ExecuteCard()
        {
            foreach (Tools.Direction dir in new Tools.Direction[]
            {
                    Tools.Direction.NorthEast,
                    Tools.Direction.NorthWest,
                    Tools.Direction.SouthEast,
                    Tools.Direction.SouthWest
            })
            {
                MapSquare temp = startSquare;
                for (int i = 0; i < level; i++)
                {
                    if (temp.HasNeighbour(dir))
                    {
                        temp = temp.GetNeighbour(dir);
                        if (temp.unit && Tools.UnitIsEnemy(temp.unit))
                            actions.Push(new DamageAction(unit, cardColor, temp.unit));
                    }
                    else
                        break;
                }
            }
            base.ExecuteCard();
        }
    }
}