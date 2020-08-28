using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Scenario0 : Scenario
    {
        public override void InitScenario()
        {
            base.InitScenario();
            scenarioInfo = "Tutorial scenario\nYour task is to first destroy the four crystals and then the following Minions.";
            winText = "You successfully defeated all minions good job!";
            loseText = "Oh no you lost, better luck next time!";
            foreach (CrystalDestroyable crystal in Object.FindObjectsOfType<CrystalDestroyable>())
                GameMaster.Instance.gearTracker.minionsList.Add(crystal);
        }

        public override void CheckEscalation()
        {
            if (GameMaster.Instance.gearTracker.minionsList.Count < 1 && escalationLevel < 2)
                Escalate();
        }

        public override void Escalate()
        {
            if (escalationLevel == 0)
            {
                Debug.Log("Escalate!");
                foreach (MapSquare sq in Object.FindObjectsOfType<MapSquare>())
                    if (sq.spawnsMinions)
                    {
                        if (sq.unit == null) // NOTE: Additional check, since the function otherwise return null which gets added to the list.
                            GameMaster.Instance.gearTracker.minionsList.Add(GameMaster.Instance.SpawnUnit(Prefabs.Instance.minion, sq));
                    }
            }

            base.Escalate();
        }

        public override bool IsGameLost => base.IsGameLost;

        public override bool IsGameWon
        {
            get
            {
                if (escalationLevel > 1)
                    return true;
                return false;
            }
        }

        public override void GenerateMap()
        {
            gameMap = Object.Instantiate(Resources.Load("Maps/Map_Scenario0"));
        }

        public override void CrystalDestroyed(CrystalDestroyable crystal)
        {
            base.CrystalDestroyed(crystal);
            GameMaster.Instance.gearTracker.minionsList.Remove(crystal);
        }

        public override void MoveMinions()
        {
            Stack<KeyValuePair<Unit, Tools.Direction>> minionMoves = new Stack<KeyValuePair<Unit, Tools.Direction>>();
            Tools.Direction[] directions = { Tools.Direction.East, Tools.Direction.North, Tools.Direction.South, Tools.Direction.West};
            Tools.Direction moveDirection = directions[Random.Range(0, directions.Length)];
            // Sort minionsList by a custom function
            foreach (Unit minion in GameMaster.Instance.gearTracker.minionsList)
                if (minion is Minion)
                {
                    minionMoves.Push(new KeyValuePair<Unit, Tools.Direction>(minion, moveDirection));
                }
            GameMaster.Instance.MoveMinions(minionMoves);
        }

        public override List<Unit> SortMinionList(List<Unit> minionList)
        {
            return minionList;
        }
    }
}