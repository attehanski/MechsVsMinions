using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class TurnState_PreGame : TurnState
    {
        public TurnState_PreGame()
        {
            stateName = "PreGame";
            UIMaster.Instance.ChangeTurnState(stateName);
        }

        public override void StartState()
        {
            base.StartState();
        }

        public override void UpdateState()
        {
            // Add reading of scenario rules here
            UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Ready);
            base.UpdateState();
        }

        public override void AdvanceState()
        {
            base.AdvanceState();

            GameMaster.Instance.currentTurnState = new TurnState_PlayerSpawn();
            GameMaster.Instance.currentTurnState.StartState();
        }
    }
}