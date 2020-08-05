using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class TurnState_MinionMove : TurnState
    {
        public TurnState_MinionMove()
        {
            stateName = "MinionMove";
            UIMaster.Instance.ChangeTurnState(stateName);
        }

        public override void StartState()
        {
            base.StartState();
            //UIMaster.Instance.commandLine.SetActive(false);
            GameMaster.Instance.scenario.MoveMinions();
        }

        public override void AdvanceState()
        {
            base.AdvanceState();

            GameMaster.Instance.currentTurnState = new TurnState_Spawn();
            GameMaster.Instance.currentTurnState.StartState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (GameMaster.Instance.minionMovesFinished)
                UIMaster.Instance.UpdateContinueButton(true);
        }
    }
}