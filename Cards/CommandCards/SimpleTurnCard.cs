using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    [System.Serializable]
    public class SimpleTurnCard : CommandCard
    {
        public SimpleTurnCard(Tools.Facing facing)
        {
            commandFacing = facing;
            text = "Turn " + facing.ToString();
        }

        public override void ExecuteCard()
        {
            base.ExecuteCard();
            GameMaster.Instance.currentPlayer.character.Turn(commandFacing);
            GameMaster.Instance.currentPlayer.character.ExecuteActions();
        }
    }
}
