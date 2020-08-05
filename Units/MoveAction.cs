using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class MoveAction : Action
    {
        protected Tools.Direction moveDirection;
        protected MapSquare neighbour;
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
            if (!actionInProgress)
            {
                neighbour = unit.mapSquare.GetNeighbour(moveDirection);
                startPos = unit.transform.position;
                actionInProgress = true;
            }

            if (!actionFinished && neighbour != null)
            {
                if (moveProgress < Tools.UnitSpeed)
                {
                    moveProgress += Time.deltaTime;
                    unit.transform.position = Vector3.Lerp(startPos, neighbour.transform.position, moveProgress / Tools.UnitSpeed);
                }
                else
                {
                    neighbour.MoveToSquare(unit, moveDirection);
                    actionFinished = true;
                }
            }
        }
    }
}
