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

        public string text;
        public Type type;
        public bool inputRequired = false;
        public Sprite textureAsset;

        // Values for input handling during card execution
        public bool inputReceived = false;
        public bool readyToExecute = false;
        protected Unit unit;
        protected MapSquare startSquare;
        protected Tools.Direction startFacing;

        public virtual void UpdateCardState()
        {
            readyToExecute = true;
        }

        public virtual void InitializeCardExecution(Unit unit)
        {

        }

        public virtual void ExecuteCard()
        {

        }

        public virtual void Input(MapSquare squareInput)
        {

        }

        /// <summary>
        /// A fail-safe function for when player faces a situation with no possible input options.
        /// </summary>
        public virtual void NoViableInputOptions()
        {
            readyToExecute = true;
        }        

        public virtual Dictionary<MapSquare, MapSquare.Interactable> GetValidInputSquares()
        {
            return new Dictionary<MapSquare, MapSquare.Interactable>();
        }
    }
}