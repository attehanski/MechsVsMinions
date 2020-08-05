using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class ChainLightning : CommandCard
    {
        private Stack<Unit> targets = new Stack<Unit>();
        private bool allTargetsChosen = false;

        public ChainLightning()
        {
            inputRequired = true;
            cardColor = Tools.Color.Yellow;
            text = "Chain Lightning";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_ChainLightning");
        }

        public override Dictionary<MapSquare, MapSquare.Interactable> GetValidInputSquares()
        {
            Dictionary<MapSquare, MapSquare.Interactable> inputSquares = new Dictionary<MapSquare, MapSquare.Interactable>();

            if (targets.Count == 0)
            {
                MapSquare frontSquare = startSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Forward, startFacing));
                KeyValuePair<MapSquare, MapSquare.Interactable> temp;
                temp = GetInteractable(frontSquare);
                inputSquares.Add(temp.Key, temp.Value);
                temp = GetInteractable(frontSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Left, startFacing)));
                inputSquares.Add(temp.Key, temp.Value);
                temp = GetInteractable(frontSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Right, startFacing)));
                inputSquares.Add(temp.Key, temp.Value);
            }
            else
            {
                foreach (Tools.Direction direction in new Tools.Direction[] {Tools.Direction.NorthEast, Tools.Direction.NorthWest, Tools.Direction.SouthEast, Tools.Direction.SouthWest })
                {
                    MapSquare square = targets.Peek().mapSquare.GetNeighbour(direction);
                    if (square && square.unit && Tools.UnitIsEnemy(square.unit) && !targets.Contains(square.unit))
                        inputSquares.Add(square, MapSquare.Interactable.Interactable);
                    else if (square)
                        inputSquares.Add(square, MapSquare.Interactable.Uninteractable);
                }
            }

            return inputSquares;
        }

        public override void Input(MapSquare squareInput)
        {
            if (targets.Count == 0)
            {
                targets.Push(squareInput.unit);
                actions.Push(new DamageAction(unit, cardColor, squareInput.unit));
            }
            else
            {
                targets.Push(squareInput.unit);
                actions.Push(new DamageAction(unit, cardColor, squareInput.unit));

                if (targets.Count >= level * 2)
                {
                    allTargetsChosen = true;
                    inputReceived = true;
                    return;
                }
            }

            inputReceived = false;
        }

        public override void UpdateCardState()
        {
            if (allTargetsChosen)
                readyToExecute = true;
        }

        public override void ExecuteCard()
        {
            targets.Clear();
            allTargetsChosen = false;
            base.ExecuteCard();
        }

        private KeyValuePair<MapSquare, MapSquare.Interactable> GetInteractable(MapSquare square)
        {
            if (square && square.unit && Tools.UnitIsEnemy(square.unit) && !targets.Contains(square.unit))
                return new KeyValuePair<MapSquare, MapSquare.Interactable>(square, MapSquare.Interactable.Interactable);
            else
                return new KeyValuePair<MapSquare, MapSquare.Interactable>(square, MapSquare.Interactable.Uninteractable);
        }
    }
}