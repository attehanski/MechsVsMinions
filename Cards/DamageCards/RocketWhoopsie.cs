using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class RocketWhoopsie : DamageCard
    {
        public RocketWhoopsie()
        {
            slottable = true;
        }

        public override void ExecuteCard()
        {
            for (int i = 0; i < 3; i++)
            {
                GameMaster.Instance.currentPlayer.character.actionStack.Push(
                    new MoveAction(
                        GameMaster.Instance.currentPlayer.character,
                        Tools.GetDirectionFromFacing(Tools.Facing.Forward, GameMaster.Instance.currentPlayer.character.facingDirection)));
            }

            GameMaster.Instance.currentPlayer.character.actionStack.Push(
                new TurnAction(
                    GameMaster.Instance.currentPlayer.character,
                    Tools.GetDirectionFromFacing(Tools.Facing.Back, GameMaster.Instance.currentPlayer.character.facingDirection)));

            GameMaster.Instance.currentPlayer.character.ExecuteActions();
        }

        public override string ToString()
        {
            return "Damage: Rocket Whoopsie";
        }
    }
}