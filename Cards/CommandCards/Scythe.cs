using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Scythe : BasicTurnCard
    {
        private List<MapSquare> targetSquares = new List<MapSquare>();
        private bool turnInputReceived = false;

        public Scythe()
        {
            cardColor = Tools.Color.Blue;
            inputRequired = true;
            text = "Scythe";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_Scythe");
        }

        public override Dictionary<MapSquare, MapSquare.Interactable> GetValidInputSquares()
        {
            if (!turnInputReceived)
                return base.GetValidInputSquares();
            else
            {
                inputSquares = new Dictionary<MapSquare, MapSquare.Interactable>();
                foreach (KeyValuePair<Tools.Direction, MapSquare> neighbour in startSquare.neighbours)
                {
                    if (!neighbour.Value.unit)
                        inputSquares.Add(neighbour.Value, MapSquare.Interactable.Uninteractable);
                    else if (Tools.UnitIsEnemy(neighbour.Value.unit) &&
                        !targetSquares.Contains(neighbour.Value))
                        inputSquares.Add(neighbour.Value, MapSquare.Interactable.Interactable);
                }
                return inputSquares;
            }
        }

        public override void Input(MapSquare squareInput)
        {
            if (!turnInputReceived)
            {
                base.Input(squareInput);
                turnInputReceived = true;
                inputReceived = false;
            }
            else
            {
                targetSquares.Add(squareInput);
                actions.Push(new DamageAction(unit, cardColor, squareInput.unit));
            }
        }

        public override void UpdateCardState()
        {
            if (targetSquares.Count == level)
                readyToExecute = true;
        }

        public override void ExecuteCard()
        {
            turnInputReceived = false;
            targetSquares.Clear();
            base.ExecuteCard();
        }
    }
}