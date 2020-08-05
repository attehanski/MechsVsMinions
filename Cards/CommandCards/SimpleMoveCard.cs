using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    [System.Serializable]
    public class SimpleMoveCard : CommandCard
    {
        public SimpleMoveCard(Tools.Facing facing, int moves)
        {
            commandFacing = facing;
            moveAmount = moves;
            text = "Move " + moves + " " + facing.ToString();
        }

        protected int moveAmount;

        public override void ExecuteCard()
        {
            base.ExecuteCard();
            for (int i = 0; i < moveAmount; i++)
                GameMaster.Instance.currentPlayer.character.Move(commandFacing);
            GameMaster.Instance.currentPlayer.character.ExecuteActions();
        }
    }
}
