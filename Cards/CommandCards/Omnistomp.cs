using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Omnistomp : CommandCard
    {
        private Tools.Direction moveDirection = Tools.Direction.None;

        public Omnistomp()
        {
            cardColor = Tools.Color.Green;
            inputRequired = true;
            text = "Omnistomp";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_Omnistomp");
        }

        public override Dictionary<MapSquare, MapSquare.Interactable> GetValidInputSquares()
        {
            inputSquares = new Dictionary<MapSquare, MapSquare.Interactable>();

            MapSquare temp = null;
            MapSquare prev = null;

            foreach (Tools.Facing facing in new Tools.Facing[] { Tools.Facing.Left, Tools.Facing.Right, Tools.Facing.Forward })
            {
                Tools.Direction moveDir = Tools.GetDirectionFromFacing(facing, startFacing);
                temp = startSquare;
                prev = null;

                for (int i = 0; i < level; i++)
                {
                    temp = temp.GetNeighbour(moveDir);

                    if (!temp.CanMoveToSquare(GameMaster.Instance.currentPlayer.character, moveDir))
                    {
                        if (prev)
                            inputSquares[prev] = MapSquare.Interactable.Interactable;
                        else
                            inputSquares.Add(temp, MapSquare.Interactable.Interactable);
                        break;
                    }

                    if (i < level - 1)
                        inputSquares.Add(temp, MapSquare.Interactable.Uninteractable);
                    else
                        inputSquares.Add(temp, MapSquare.Interactable.Interactable);

                    prev = temp;
                }
            }

            return inputSquares;
        }

        public override void Input(MapSquare squareInput)
        {
            moveDirection = Tools.GetDirection(startSquare, squareInput);
            inputReceived = true;
        }

        public override void UpdateCardState()
        {
            if (inputReceived)
                readyToExecute = true;
        }

        public override void ExecuteCard()
        {
            // If no available moves, return
            if (moveDirection == Tools.Direction.None)
                return;

            MapSquare temp = startSquare.GetNeighbour(moveDirection);

            for (int i = 1; i <= level; i++)
            {
                if (temp != null && temp.CanMoveToSquare(unit, moveDirection))
                {
                    unit.actionStack.Push(new MoveAction(unit, moveDirection));
                    temp = temp.GetNeighbour(moveDirection);
                }
                else
                    break;
            }

            base.ExecuteCard();
        }
    }
}