using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    [System.Serializable]
    public abstract class Scenario
    {
        public int escalationLevel = 0;

        public virtual void InitScenario()
        {

        }

        public virtual void CheckEscalation()
        {

        }

        public virtual void Escalate()
        {
            escalationLevel++;
        }

        public virtual bool GameWon
        {
            // Win condition code here
            get { return false; }
        }

        public virtual bool GameLost
        {
            // Lose condition code here
            get { return false; }
        }

        public virtual void GenerateMap()
        {
            // Instantiate correct map from Resources
            // NOTE: Could also use Prefabs for this
        }

        public virtual void CrystalDestroyed(CrystalDestroyable crystal)
        {

        }

        // NOTE: This might not be necessary, can just be added as a check inside MoveMinions
        public virtual void MinionReachedSchool() 
        {

        }


        public virtual void MoveMinions()
        {
            UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Ready);
            // This should include:
            // - the order the minions move in
            // - calculating the direction for each minion move
            // - send order to GameMaster with a stack/queue of moves to handle
        }

        public virtual List<Unit> SortMinionList(List<Unit> minionList)
        {
            return minionList;
        }
    }
}
