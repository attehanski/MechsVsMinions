using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class TurnState_EndTurn : TurnState
    {
        public TurnState_EndTurn()
        {
            stateName = "EndTurn";
            UIMaster.Instance.ChangeTurnState(stateName);
        }

        public override void StartState()
        {
            base.StartState();
            UIMaster.Instance.commandLine.SetActive(true);
            GameMaster.Instance.scenario.CheckEscalation();
        }

        public override void AdvanceState()
        {
            base.AdvanceState();

            GameMaster.Instance.turnNumber++;
            GameMaster.Instance.currentTurnState = new TurnState_Draft();
            GameMaster.Instance.currentTurnState.StartState();
        }

        public override void UpdateState()
        {
            UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Ready);
            base.UpdateState();
        }
    }
}