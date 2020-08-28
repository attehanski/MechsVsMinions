using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    [System.Serializable]
    public class Player
    {
        public string playerName;
        public CommandLine commandLine;
        public Champion character;        
        public List<Card> hand = new List<Card>();
        public Card currentCard;
        public bool ready = false;
        
        public Player(string name)
        {
            playerName = name;
            commandLine = new CommandLine();
        }

        public void ResetPlayer()
        {
            Debug.Log("ResetPlayer!");
            currentCard = null;
            hand.Clear();
            character = null;
            commandLine = null;
        }

        public void SpawnChampion(MapSquare square, Tools.Direction facing)
        {
            character = GameMaster.Instance.SpawnUnit(Prefabs.Instance.genericChampion, square) as Champion;
            character.facingDirection = facing;
            character.model.transform.eulerAngles = Tools.GetRotation(facing);
        }
    }
}
