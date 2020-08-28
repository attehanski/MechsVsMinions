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

            inputSquares.Add(startSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Left, startFacingDirection)), MapSquare.Interactable.ActiveChoice);
            inputSquares.Add(startSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Right, startFacingDirection)), MapSquare.Interactable.ActiveChoice);
            if (level > 1)
                inputSquares.Add(startSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Back, startFacingDirection)), MapSquare.Interactable.ActiveChoice);
            if (level > 2)
                inputSquares.Add(startSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Forward, startFacingDirection)), MapSquare.Interactable.ActiveChoice);

            return inputSquares;
        }

        public override void UpdateCardState()
        {
            if (actionsExecuted)
                cardState = CardState.Finished;
        }

        public override void Input(MapSquare squareInput)
        {
            unit.Turn(unit.mapSquare.GetNeighbourDirection(squareInput));
        }

        public override void ExecuteCard()
        {
            base.ExecuteCard();
        }
    }
}