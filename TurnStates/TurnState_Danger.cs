using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class TurnState_Danger : TurnState
    {
        public TurnState_Danger()
        {
            stateName = "DangerPhase";
            UIMaster.Instance.ChangeTurnState(stateName);
        }

        public override void StartState()
        {
            base.StartState();
        }

        public override void AdvanceState()
        {
            base.AdvanceState();

            GameMaster.Instance.currentTurnState = new TurnState_EndTurn();
            GameMaster.Instance.currentTurnState.StartState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            UIMaster.Instance.UpdateContinueButton(true);
        }
    }
}