using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class TestScenario : Scenario
    {
        public override void InitScenario()
        {
            base.InitScenario();
            scenarioInfo = "This scenario is for testing purposes.\nThe scenario will end when you have defeated 25 Minions.\nYou lose if you take more than 20 turns.";
            winText = "You defeated enough minions!";
            loseText = "Oh no you took too long to kill the minions, better luck next time!";
        }

        public override bool IsGameWon
        {
            get
            {
                if (GameMaster.Instance.gearTracker.minionsKilled >= 25)
                    return true;
                else
                    return false;
            }
        }

        public override bool IsGameLost
        {
            get
            {
                if (GameMaster.Instance.turnNumber > 20)
                    return true;
                else
                    return false;
            }
        }

        public override void GenerateMap()
        {
            gameMap = Object.Instantiate(Resources.Load("Maps/Map_TestScenario"));
        }
    }
}