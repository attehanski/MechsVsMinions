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
            Debug.Log("Execute Stuck Controls!");
            if (isMove)
                GameMaster.Instance.currentPlayer.character.Move(direction);
            else
                GameMaster.Instance.currentPlayer.character.Turn(direction);

            GameMaster.Instance.currentPlayer.character.ExecuteActions();
            base.ExecuteCard();
        }

        public override string ToString()
        {
            return "Damage: Stuck Controls " + (isMove ? "Move " : "Turn ") + direction;
        }
    }
}