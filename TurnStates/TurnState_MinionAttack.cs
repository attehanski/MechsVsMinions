using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class TurnState_MinionAttack : TurnState
    {
        public bool damageInAction = false;
        private bool minionAttackFinished = false;

        public TurnState_MinionAttack()
        {
            stateName = "MinionAttack";
            UIMaster.Instance.ChangeTurnState(stateName);
        }

        public override void StartState()
        {
            base.StartState();
            UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Inactive);
        }

        public override void AdvanceState()
        {
            base.AdvanceState();

            GameMaster.Instance.currentTurnState = new TurnState_Danger();
            GameMaster.Instance.currentTurnState.StartState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (!minionAttackFinished)
            {
                if (!GameMaster.Instance.GetAllPlayersReady() && !damageInAction)
                {
                    GameMaster.Instance.DoPlayerDamage(GameMaster.Instance.currentPlayer); // NOTE: Change to cycle through all players
                    damageInAction = true;
                }
            }
        }

        public override void AllPlayersReady()
        {
            if (minionAttackFinished)
            {
                base.AllPlayersReady();
            }
            else
            {
                GameMaster.Instance.SetAllPlayersReady(false);
                minionAttackFinished = true;
                UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Ready);
            }
        }

    }
}