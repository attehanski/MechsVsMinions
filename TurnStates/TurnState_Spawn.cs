using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class TurnState_Spawn : TurnState
    {
        public TurnState_Spawn()
        {
            stateName = "MinionSpawn";
            UIMaster.Instance.ChangeTurnState(stateName);
        }

        public override void StartState()
        {
            base.StartState();
        }

        public override void AdvanceState()
        {
            base.AdvanceState();

            GameMaster.Instance.currentTurnState = new TurnState_MinionAttack();
            GameMaster.Instance.currentTurnState.StartState();
        }

        public override void UpdateState()
        {
            UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Ready);
            base.UpdateState();
        }
    }
}