﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class MajorGlitch : DamageCard
    {
        public override void ExecuteCard()
        {
            CommandLine cmdLine = GameMaster.Instance.currentPlayer.commandLine;
            CardStack<Card> temp = cmdLine.cards[0];
            cmdLine.cards[0] = cmdLine.cards[5];
            cmdLine.cards[5] = temp;

            temp = cmdLine.cards[1];
            cmdLine.cards[1] = cmdLine.cards[4];
            cmdLine.cards[4] = temp;
            
            temp = cmdLine.cards[2];
            cmdLine.cards[2] = cmdLine.cards[3];
            cmdLine.cards[3] = temp;

            UIMaster.Instance.UpdateCommandLine();
        }

        public override string ToString()
        {
            return "Damage: Major Glitch";
        }
    }
}