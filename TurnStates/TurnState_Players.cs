using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class TurnState_Players : TurnState
    {
        public TurnState_Players()
        {
            stateName = "PlayerTurn";
            UIMaster.Instance.ChangeTurnState(stateName);
        }

        public override void StartState()
        {
            base.StartState();
            GameMaster.Instance.ExecuteAllCommandLines();
        }

        public override void AdvanceState()
        {
            base.AdvanceState();

            GameMaster.Instance.currentTurnState = new TurnState_MinionMove();
            GameMaster.Instance.currentTurnState.StartState();
        }        
    }
}