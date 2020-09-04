using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class BasicMoveCard : CommandCard
    {
        public Tools.Facing moveFacing;

        protected Tuple<int, int> minMaxMoves;
        protected int doneMoves;
        protected bool encounteredUntenterableSquare = false;

        public BasicMoveCard()
        {
            inputRequired = true;
        }

        public override void InitializeCardExecution(Unit executingUnit)
        {
            base.InitializeCardExecution(executingUnit);
            moveFacing = Tools.Facing.Forward;
            minMaxMoves = new Tuple<int, int>(level, level);
            doneMoves = 0;
            encounteredUntenterableSquare = false;
        }

        public override void UpdateCardState()
        {
            if (doneMoves >= minMaxMoves.Item2)
                cardState = CardState.Finished;
        }

        public override Dictionary<MapSquare, MapSquare.Interactable> GetValidInputSquares()
        {
            CheckTowingStatus();
            inputSquares = new Dictionary<MapSquare, MapSquare.Interactable>();

            foreach (KeyValuePair<MapSquare, MapSquare.Interactable> kvp in GetInputsForOneDirection(startFacingDirection))
                inputSquares.Add(kvp.Key, kvp.Value);
            
            return inputSquares;
        }

        public override void Input(MapSquare inputSquare)
        {
            MapSquare temp = unit.mapSquare;
            Tools.Direction moveDirection = Tools.GetDirectionFromFacing(moveFacing, startFacingDirection);

            while (temp != inputSquare)
            {

                if (unit.CanMove(moveDirection))
                    doneMoves++;
                else
                    doneMoves = minMaxMoves.Item2;

                temp = temp.GetNeighbour(moveDirection);
                actions.Push(new MoveAction(unit, moveDirection));
            }
        }

        protected Dictionary<MapSquare, MapSquare.Interactable> GetInputsForOneDirection(Tools.Direction targetDirection)
        {
            Dictionary<MapSquare, MapSquare.Interactable> directionInputSquares = new Dictionary<MapSquare, MapSquare.Interactable>();
            MapSquare temp = unit.mapSquare.GetNeighbour(targetDirection);
            encounteredUntenterableSquare = false;

            for (int i = 1; i <= minMaxMoves.Item2 - doneMoves; i++)
            {
                if (temp != null)
                {
                    if (encounteredUntenterableSquare)
                        directionInputSquares.Add(temp, MapSquare.Interactable.InactiveChoice);
                    else if (i + doneMoves < minMaxMoves.Item1)
                        directionInputSquares.Add(temp, MapSquare.Interactable.NonfinalChoice);
                    else if (i + doneMoves <= minMaxMoves.Item2)
                    {
                        directionInputSquares.Add(temp, MapSquare.Interactable.ActiveChoice);
                        if (!temp.CanEnterSquare(unit, targetDirection))
                            encounteredUntenterableSquare = true;
                    }
                    
                    temp = temp.GetNeighbour(targetDirection);
                }
                else
                    break;
            }
            return directionInputSquares;
        }

        public override void ExecuteCard()
        {
            base.ExecuteCard();
            if (doneMoves < minMaxMoves.Item2)
            {
                if (doneMoves >= minMaxMoves.Item1)
                    UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Ready);
            }
        }

        #region Towing
        protected void CheckTowingStatus()
        {
            Debug.Log("Checking tow status!");
            List<Unit> towables = GetTowableNeighbours();
            if (TowIsPossible(towables))
                UIMaster.Instance.towButton.SetActive(true);
            else
                UIMaster.Instance.towButton.SetActive(false);
        }

        private bool TowIsPossible(List<Unit> towableNeighbours)
        {
            if (minMaxMoves.Item2 - doneMoves >= 2 && towableNeighbours.Count > 0)
                return true;
            return false;
        }

        private List<Unit> GetTowableNeighbours()
        {
            List<Unit> towableNeighbours = new List<Unit>();
            foreach (KeyValuePair<Tools.Direction, MapSquare> neighbour in unit.mapSquare.neighbours)
                if (Tools.DirectionIsCardinal(neighbour.Key) && neighbour.Value.unit && neighbour.Value.unit.towable)
                    towableNeighbours.Add(neighbour.Value.unit);
            return towableNeighbours;
        }
        #endregion
    }
}