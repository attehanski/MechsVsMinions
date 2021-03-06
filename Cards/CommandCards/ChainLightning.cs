﻿using System.Collections;
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

        public override void InitializeCardExecution(Unit executingUnit)
        {
            base.InitializeCardExecution(executingUnit);
            targets.Clear();
            allTargetsChosen = false;
        }

        public override Dictionary<MapSquare, MapSquare.Interactable> GetValidInputSquares()
        {
            Dictionary<MapSquare, MapSquare.Interactable> inputSquares = new Dictionary<MapSquare, MapSquare.Interactable>();

            if (targets.Count == 0)
            {
                MapSquare frontSquare = startSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Forward, startFacingDirection));
                KeyValuePair<MapSquare, MapSquare.Interactable> temp;
                temp = GetInteractable(frontSquare);
                inputSquares.Add(temp.Key, temp.Value);
                temp = GetInteractable(frontSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Left, startFacingDirection)));
                inputSquares.Add(temp.Key, temp.Value);
                temp = GetInteractable(frontSquare.GetNeighbour(Tools.GetDirectionFromFacing(Tools.Facing.Right, startFacingDirection)));
                inputSquares.Add(temp.Key, temp.Value);
            }
            else
            {
                foreach (Tools.Direction direction in new Tools.Direction[] {Tools.Direction.NorthEast, Tools.Direction.NorthWest, Tools.Direction.SouthEast, Tools.Direction.SouthWest })
                {
                    MapSquare square = targets.Peek().mapSquare.GetNeighbour(direction);
                    if (square && square.unit && Tools.UnitIsEnemy(square.unit) && !targets.Contains(square.unit))
                        inputSquares.Add(square, MapSquare.Interactable.ActiveChoice);
                    else if (square)
                        inputSquares.Add(square, MapSquare.Interactable.InactiveChoice);
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
                    return;
                }
            }
            
        }

        public override void UpdateCardState()
        {
            if (allTargetsChosen)
            {
                cardState = CardState.Finished;
            }
        }

        public override void ExecuteCard()
        {
            base.ExecuteCard();
        }

        private KeyValuePair<MapSquare, MapSquare.Interactable> GetInteractable(MapSquare square)
        {
            if (square && square.unit && Tools.UnitIsEnemy(square.unit) && !targets.Contains(square.unit))
                return new KeyValuePair<MapSquare, MapSquare.Interactable>(square, MapSquare.Interactable.ActiveChoice);
            else
                return new KeyValuePair<MapSquare, MapSquare.Interactable>(square, MapSquare.Interactable.InactiveChoice);
        }
    }
}