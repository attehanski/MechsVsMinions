using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class TestScenario : Scenario
    {
        public override bool GameWon
        {
            get
            {
                if (GameMaster.Instance.gearTracker.minionsKilled >= 7)
                    return true;
                else
                    return false;
            }
        }

        public override bool GameLost
        {
            get
            {
                if (GameMaster.Instance.turnNumber > 10)
                    return true;
                else
                    return false;
            }
        }

        public override void GenerateMap()
        {
            Object.Instantiate(Resources.Load("Maps/Map_TestScenario"));
        }
    }
}