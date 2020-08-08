using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    [System.Serializable]
    public abstract class TurnState
    {
        public string stateName;
        public bool stateFinished = false;

        public TurnState()
        {
            UIMaster.Instance.ChangeTurnState(stateName);
        }

        public virtual void StartState()
        {
            GameMaster.Instance.UpdateUIState();
        }

        public virtual void UpdateState()
        {
            if (stateFinished)
                AdvanceState();
        }

        public virtual void AllPlayersReady()
        {
            // When all players are ready in this state, do something
            GameMaster.Instance.FinishState();
        }

        public virtual void AdvanceState()
        {
            //Debug.Log("Advance state " + stateName);
        }

    }
}