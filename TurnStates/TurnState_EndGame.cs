using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class TurnState_EndGame : TurnState
    {
        public TurnState_EndGame(bool gameWon)
        {
            stateName = "EndGame";
            if (gameWon)
                Debug.Log("YOU WON THE GAME!");
            else
                Debug.Log("YOU LOST THE GAME! :(");
        }
    }
}