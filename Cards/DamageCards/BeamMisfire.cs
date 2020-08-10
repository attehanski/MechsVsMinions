using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class BeamMisfire : DamageCard
    {
        public BeamMisfire()
        {
            slottable = false;
        }

        public override void ExecuteCard()
        {
            Tools.Direction[] directions = { Tools.Direction.North, Tools.Direction.East, Tools.Direction.West, Tools.Direction.South };
            foreach (Tools.Direction direction in directions)
            {
                MapSquare temp = GameMaster.Instance.currentPlayer.character.mapSquare;
                while (temp.HasNeighbour(direction))
                {
                    temp = temp.GetNeighbour(direction);
                    if (temp.unit != null && temp.unit is Champion)
                        temp.unit.TakeDamage();
                }
            }
        }

        public override string ToString()
        {
            return "Damage: Beam Misfire";
        }
    }
}