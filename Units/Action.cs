using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    [System.Serializable]
    public abstract class Action
    {
        public bool actionInProgress = false;
        public bool actionFinished = false;
        protected Unit unit;

        public virtual void UpdateState()
        {

        }
    }
}
