using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class StuckControls : DamageCard
    {
        private Tools.Facing direction;
        private bool isMove = false;

        public StuckControls(Tools.Facing damageDirection, bool isMoveDamage)
        {
            direction = damageDirection;
            isMove = isMoveDamage;
            text = (isMove ? "Move " : "Turn ") + direction;
            slottable = true;
        }

        public override void ExecuteCard()
        {
            if (isMove)
                GameMaster.Instance.currentPlayer.character.actionStack.Push(
                    new MoveAction(
                        GameMaster.Instance.currentPlayer.character, 
                        Tools.GetDirectionFromFacing(direction, GameMaster.Instance.currentPlayer.character.facingDirection)));
            else
                GameMaster.Instance.currentPlayer.character.actionStack.Push(
                    new TurnAction(
                        GameMaster.Instance.currentPlayer.character,
                        Tools.GetDirectionFromFacing(direction, GameMaster.Instance.currentPlayer.character.facingDirection)));

            GameMaster.Instance.currentPlayer.character.ExecuteActions();
        }

        public override string ToString()
        {
            return "Damage: Stuck Controls " + (isMove ? "Move " : "Turn ") + direction;
        }
    }
}