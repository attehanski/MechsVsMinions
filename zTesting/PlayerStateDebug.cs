﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class PlayerStateDebug : MonoBehaviour
    {
        [System.Serializable]
        public class PlayerCommandLine
        {
            public Card[] slot1;
            public Card[] slot2;
            public Card[] slot3;
            public Card[] slot4;
            public Card[] slot5;
            public Card[] slot6;
        }

        public string playerName;
        public Champion character;
        public Card currentCard;
        public bool ready = false;
        public PlayerCommandLine cmdLine;
        public Card[] deck;
        public Card[] discard;
        public Card[] damageDeck;
        public Card[] damageDiscard;
        
        void Update()
        {
            if (GameMaster.Instance.localPlayer == null)
                return;

            playerName = GameMaster.Instance.localPlayer.playerName;
            character = GameMaster.Instance.localPlayer.character;
            currentCard = GameMaster.Instance.localPlayer.currentCard;
            ready = GameMaster.Instance.localPlayer.ready;
            deck = GameMaster.Instance.commmandCardDeck.ToArray();
            discard = GameMaster.Instance.commandCardDiscard.ToArray();
            damageDeck = GameMaster.Instance.damageCardDeck.ToArray();
            damageDiscard = GameMaster.Instance.damageCardDiscard.ToArray();
            
            for (int i = 0; i < 6; i++)
            {
                switch (i)
                {
                    case 0:
                        cmdLine.slot1 = GameMaster.Instance.localPlayer.commandLine.cards[i].ToArray();
                        break;
                    case 1:
                        cmdLine.slot2 = GameMaster.Instance.localPlayer.commandLine.cards[i].ToArray();
                        break;
                    case 2:
                        cmdLine.slot3 = GameMaster.Instance.localPlayer.commandLine.cards[i].ToArray();
                        break;
                    case 3:
                        cmdLine.slot4 = GameMaster.Instance.localPlayer.commandLine.cards[i].ToArray();
                        break;
                    case 4:
                        cmdLine.slot5 = GameMaster.Instance.localPlayer.commandLine.cards[i].ToArray();
                        break;
                    case 5:
                        cmdLine.slot6 = GameMaster.Instance.localPlayer.commandLine.cards[i].ToArray();
                        break;
                }
            }
        }
    }
}