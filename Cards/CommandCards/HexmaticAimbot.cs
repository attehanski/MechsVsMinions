using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class HexmaticAimbot : CommandCard
    {
        public HexmaticAimbot()
        {
            inputRequired = true;
            cardColor = Tools.Color.Green;
            text = "Hexmatic Aimbot";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_HexmaticAimbot");
        }

        public override Dictionary<MapSquare, MapSquare.Interactable> GetValidInputSquares()
        {
            Dictionary<MapSquare, MapSquare.Interactable> inputSquares = new Dictionary<MapSquare, MapSquare.Interactable>();

            foreach (MapSquare square in GetNeighboursRecursive(startSquare, level))
            {
                if (!inputSquares.ContainsKey(square))
                {
                    if (square.unit && Tools.UnitIsEnemy(square.unit))
                        inputSquares.Add(square, MapSquare.Interactable.ActiveChoice);
                    else
                        inputSquares.Add(square, MapSquare.Interactable.InactiveChoice);
                }
            }

            return inputSquares;
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

        public override void Input(MapSquare squareInput)
        {
            actions.Push(new DamageAction(unit, cardColor, squareInput.unit));
        }

        public override void UpdateCardState()
        {
            if (actionsExecuted)
                cardState = CardState.Finished;
        }
    }
}