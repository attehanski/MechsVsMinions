using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class TurnState_PlayerSpawn : TurnState
    {
        private class PlayerSpawnData
        {
            public MapSquare spawnPosition;
            public MapSquare spawnFacing;
        }

        private List<MapSquare> spawnSquares = new List<MapSquare>();
        private List<MapSquare> interactableSquares = new List<MapSquare>();
        private PlayerSpawnData spawn;

        public TurnState_PlayerSpawn()
        {
            stateName = "PlayerSpawn";
            UIMaster.Instance.ChangeTurnState(stateName);
        }

        public override void StartState()
        {
            base.StartState();

            UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Inactive);

            foreach (MapSquare square in Object.FindObjectsOfType<MapSquare>())
                if (square.playerSpawn)
                {
                    spawnSquares.Add(square);
                    interactableSquares.Add(square);
                    square.interactable = MapSquare.Interactable.ActiveChoice;
                }
            spawn = new PlayerSpawnData();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if (!stateFinished)
            {
                // If current player has a character spawned, move to next or finish spawn phase
                if (GameMaster.Instance.currentPlayer.character != null)
                {
                    if (AllPlayersSpawned())
                    {
                        UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Ready);
                        return;
                    }

                    // Not all players are spawned, move to the next
                    GameMaster.Instance.currentPlayer = GameMaster.Instance.players[GameMaster.Instance.players.IndexOf(GameMaster.Instance.currentPlayer) + 1];
                    spawnSquares.Remove(GameMaster.Instance.currentPlayer.character.mapSquare);
                    foreach (MapSquare square in spawnSquares)
                    {
                        interactableSquares.Add(square);
                        square.interactable = MapSquare.Interactable.ActiveChoice;
                    }
                    spawn = new PlayerSpawnData();
                }

                // If we have an interacted square, check first if it was a position or rotation. Get the data, then update the interactable squares
                // NOTE: This is a huge mess and warrants a larger cleanup
                if (GameMaster.Instance.interactedSquare != null)
                {
                    if (spawn.spawnPosition == null)
                    {
                        spawn.spawnPosition = GameMaster.Instance.interactedSquare;

                        foreach (MapSquare square in interactableSquares)
                            square.interactable = MapSquare.Interactable.Passive;

                        interactableSquares.Clear();

                        interactableSquares.Add(spawn.spawnPosition.GetNeighbour(Tools.Direction.East));
                        interactableSquares.Add(spawn.spawnPosition.GetNeighbour(Tools.Direction.West));
                        interactableSquares.Add(spawn.spawnPosition.GetNeighbour(Tools.Direction.North));
                        interactableSquares.Add(spawn.spawnPosition.GetNeighbour(Tools.Direction.South));

                        foreach (MapSquare square in interactableSquares)
                            square.interactable = MapSquare.Interactable.ActiveChoice;
                    }
                    else if (spawn.spawnFacing == null)
                    {
                        spawn.spawnFacing = GameMaster.Instance.interactedSquare;
                        GameMaster.Instance.currentPlayer.SpawnChampion(spawn.spawnPosition, spawn.spawnPosition.GetNeighbourDirection(spawn.spawnFacing));

                        foreach (MapSquare square in interactableSquares)
                            square.interactable = MapSquare.Interactable.Passive;

                        interactableSquares.Clear();
                    }
                    GameMaster.Instance.interactedSquare = null;
                }
            }
        }

        public override void AdvanceState()
        {
            base.AdvanceState();

            GameMaster.Instance.currentTurnState = new TurnState_DoubleDraft();
            //GameMaster.Instance.currentTurnState = new TurnState_Draft();
            GameMaster.Instance.currentTurnState.StartState();
        }

        private bool AllPlayersSpawned()
        {
            bool allSpawned = true;
            foreach (Player player in GameMaster.Instance.players)
                if (!player.character)
                    allSpawned = false;

            return allSpawned;
        }
    }
}