using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Scythe : BasicTurnCard
    {
        private int damageActionsDone = 0;
        private bool turnInputReceived = false;

        public Scythe()
        {
            cardColor = Tools.Color.Blue;
            text = "Scythe";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_Scythe");
        }

        public override void InitializeCardExecution(Unit executingUnit)
        {
            base.InitializeCardExecution(executingUnit);
            turnInputReceived = false;
            damageActionsDone = 0;
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
                        inputSquares.Add(neighbour.Value, MapSquare.Interactable.InactiveChoice);
                    else if (Tools.UnitIsEnemy(neighbour.Value.unit))
                        inputSquares.Add(neighbour.Value, MapSquare.Interactable.ActiveChoice);
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
            }
            else
            {
                damageActionsDone++;
                actions.Push(new DamageAction(unit, cardColor, squareInput.unit));
            }
        }

        public override void UpdateCardState()
        {
            if (damageActionsDone == level)
            {
                cardState = CardState.Finished;
            }
            else
                cardState = CardState.RequiresInput;
        }

        public override void ExecuteCard()
        {
            base.ExecuteCard();
        }
    }
}