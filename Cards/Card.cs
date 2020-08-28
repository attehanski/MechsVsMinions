using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    [System.Serializable]
    public class Card
    {
        public enum Type
        {
            Command,
            Damage,
            Schematic,
            Boss
        }

        public enum CardState
        {
            Finished,
            RequiresInput,
            InputOrFinish,
            InputReceived,
            NoInputRequired
        }

        public string text;
        public Type type;
        public bool inputRequired = false;
        public Sprite textureAsset;

        // Values for input handling during card execution
        public CardState cardState = CardState.RequiresInput;
        protected Unit unit;
        protected MapSquare startSquare;
        protected Tools.Direction startFacingDirection;

        protected bool actionsExecuted = false;

        public virtual void UpdateCardState()
        {
            cardState = CardState.Finished;
        }

        public virtual void InitializeCardExecution(Unit executingUnit)
        {
            cardState = CardState.RequiresInput;
            unit = executingUnit;
            startSquare = executingUnit.mapSquare;
            startFacingDirection = executingUnit.facingDirection;
        }

        public virtual void ExecuteCard()
        {
            actionsExecuted = true;
        }

        public virtual void Input(MapSquare squareInput)
        {

        }

        /// <summary>
        /// A fail-safe function for when player faces a situation with no possible input options.
        /// </summary>
        public virtual void NoViableInputOptions()
        {
            Debug.Log("NoViableInputOptions!");
            cardState = CardState.Finished;
        }        

        public virtual Dictionary<MapSquare, MapSquare.Interactable> GetValidInputSquares()
        {
            return new Dictionary<MapSquare, MapSquare.Interactable>();
        }
    }
}