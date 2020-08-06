using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class BasicTurnCard : CommandCard
    {
        public BasicTurnCard()
        {
            cardColor = Tools.Color.Red;
            text = "Turn 90 degrees";
            inputRequired = true;
        }

        public override void InitializeCardExecution(Unit executingUnit)
        {
            base.InitializeCardExecution(executingUnit);
        }

        public override Dictionary<MapSquare, MapSquare.Interactable> GetValidInputSquares()
        {
            inputSquares = new Dictionary<MapSquare, MapSquare.Interactable>();

            inputSquares.Add(startSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Left, startFacing)), MapSquare.Interactable.Interactable);
            inputSquares.Add(startSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Right, startFacing)), MapSquare.Interactable.Interactable);
            if (level > 1)
                inputSquares.Add(startSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Back, startFacing)), MapSquare.Interactable.Interactable);
            if (level > 2)
                inputSquares.Add(startSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Forward, startFacing)), MapSquare.Interactable.Interactable);

            return inputSquares;
        }

        public override void UpdateCardState()
        {
            if (inputReceived)
                readyToExecute = true;
        }

        public override void Input(MapSquare squareInput)
        {
            unit.Turn(unit.mapSquare.GetNeighbourDirection(squareInput));
            inputReceived = true;
        }

        public override void ExecuteCard()
        {
            base.ExecuteCard();
        }
    }
}