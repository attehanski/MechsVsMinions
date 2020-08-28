using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Speed : BasicMoveCard
    {
        public Speed()
        {
            cardColor = Tools.Color.Yellow;
            inputRequired = true;
            text = "Speed";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_Speed");
        }

        public override void InitializeCardExecution(Unit executingUnit)
        {
            base.InitializeCardExecution(executingUnit);
            minMaxMoves = new Tuple<int, int>(level, level*2);
        }

        public override void UpdateCardState()
        {
            base.UpdateCardState();
            if (GameMaster.Instance.currentPlayer.ready)
                cardState = CardState.Finished;
        }
    }
}