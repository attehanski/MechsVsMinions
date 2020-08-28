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
                GameMaster.Instance.scenario.GameWon();
            else
                GameMaster.Instance.scenario.GameLost();
        }
    }
}