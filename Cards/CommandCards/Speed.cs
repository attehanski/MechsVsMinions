﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Speed : CommandCard
    {
        private Vector2 minMaxMoves;

        public Speed()
        {
            cardColor = Tools.Color.Yellow;
            inputRequired = true;
            text = "Speed";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_Speed");
        }

        public override void InitializeCardExecution(Unit executingUnit)
        {
            base.InitializeCardExecution(executingUnit);
            minMaxMoves = new Vector2(level, level * 2);
        }

        public override Dictionary<MapSquare, MapSquare.Interactable> GetValidInputSquares()
        {
            inputSquares = new Dictionary<MapSquare, MapSquare.Interactable>();
            MapSquare temp = startSquare.GetNeighbour(startFacing);
            
            for (int i = 1; i <= minMaxMoves.y; i++)
            {
                if (temp != null)
                {
                    if (i < minMaxMoves.x)
                        inputSquares.Add(temp, MapSquare.Interactable.Uninteractable);
                    else if (i <= minMaxMoves.y && temp.CanMoveToSquare(unit, startFacing))
                        inputSquares.Add(temp, MapSquare.Interactable.Interactable);

                    temp = temp.GetNeighbour(startFacing);
                }
                else
                    break;
            }
            return inputSquares;
        }

        public override void UpdateCardState()
        {
            if (inputReceived)
                readyToExecute = true;
        }

        public override void Input(MapSquare squareInput)
        {
            MapSquare temp = startSquare;
            while (temp != squareInput)
            {
                unit.actionStack.Push(new MoveAction(unit, startFacing));
                temp = temp.GetNeighbour(startFacing);
            }
            inputReceived = true;
        }

        public override void ExecuteCard()
        {
            base.ExecuteCard();
        }

        public override void NoViableInputOptions()
        {
            MapSquare temp = startSquare.GetNeighbour(startFacing);

            for (int i = 1; i <= minMaxMoves.y; i++)
            {
                if (temp != null && temp.CanMoveToSquare(unit, startFacing))
                {
                    unit.actionStack.Push(new MoveAction(unit, startFacing));
                    temp = temp.GetNeighbour(startFacing);
                }
                else
                    break;
            }

            base.NoViableInputOptions();
        }
    }
}