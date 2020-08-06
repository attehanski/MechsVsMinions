using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class MoveAction : Action
    {
        protected Tools.Direction moveDirection;
        protected MapSquare targetSquare;
        protected float moveProgress = 0f;
        protected Vector3 startPos;

        public MoveAction(Unit targetUnit, Tools.Direction direction)
        {
            unit = targetUnit;
            moveDirection = direction;
            moveProgress = 0f;
        }

        public override void UpdateState()
        {
            base.UpdateState();

            // First initialize and handle the logic
            if (!actionInProgress)
            {
                startPos = unit.transform.position;
                unit.AttemptMove(moveDirection);
                targetSquare = unit.mapSquare;
                actionInProgress = true;
            }

            // Handle animating the movement
            if (!actionFinished && targetSquare != null)
            {
                if (moveProgress < Tools.UnitSpeed)
                {
                    moveProgress += Time.deltaTime;
                    unit.transform.position = Vector3.Lerp(startPos, targetSquare.transform.position, moveProgress / Tools.UnitSpeed);
                }
                else
                {
                    actionFinished = true;
                }
            }
        }
    }
}
