using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class FuelTank : BasicTurnCard
    {
        public FuelTank()
        {
            cardColor = Tools.Color.Red;
            inputRequired = true;
            text = "Fuel Tank";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_FuelTank");
        }

        public void PreventDamage()
        {
            List<MapSquare> damagedSquares = new List<MapSquare>();

            // Explode and deal damage to surrounding enemies
            foreach (MapSquare square in GetNeighboursRecursive(startSquare, level))
            {
                if (!damagedSquares.Contains(square))
                {
                    if (square.unit && Tools.UnitIsEnemy(square.unit))
                    {
                        damagedSquares.Add(square);
                        square.unit.TakeDamage(cardColor);
                    }
                }
            }

            // Discard stack

        }

        private List<MapSquare> GetNeighboursRecursive(MapSquare square, int recursion)
        {
            List<MapSquare> neighbourList = new List<MapSquare>();
            foreach (KeyValuePair<Tools.Direction, MapSquare> neighbour in square.neighbours)
            {
                neighbourList.Add(neighbour.Value);
                if (recursion > 1)
                    neighbourList.AddRange(GetNeighboursRecursive(neighbour.Value, recursion - 1));
            }

            return neighbourList;
        }
    }
}