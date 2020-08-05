using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class SquareHighlightTest : MonoBehaviour
    {
        public MapSquare[] interactables;
        public MapSquare[] uninteractables;
        public bool trigger = false;
        
        void Update()
        {
            if (trigger)
            {
                foreach (MapSquare sq in interactables)
                    sq.interactable = MapSquare.Interactable.Interactable;

                foreach (MapSquare sq in uninteractables)
                    sq.interactable = MapSquare.Interactable.Uninteractable;

                trigger = false;
            }
        }
    }
}