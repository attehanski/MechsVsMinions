using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class TurnAction : Action
    {
        protected Tools.Direction goalDirection;
        protected Quaternion startRot;
        protected Vector3 goalRotation;
        protected float rotationProgress = 0f;

        public TurnAction(Unit targetUnit, Tools.Direction direction)
        {
            goalDirection = direction;
            goalRotation = Tools.GetRotation(goalDirection);
            unit = targetUnit;
        }

        public TurnAction(Unit targetUnit, Tools.Facing facing)
        {
            goalDirection = Tools.GetDirectionFromFacing(facing, unit.facingDirection);
            goalRotation = Tools.GetRotation(goalDirection);
            unit = targetUnit;
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (!actionInProgress)
            {
                startRot = unit.model.transform.rotation;
                actionInProgress = true;
            }

            if (!actionFinished)
            {
                if (rotationProgress < Tools.UnitRotationSpeed)
                {
                    rotationProgress += Time.deltaTime;
                    unit.model.transform.rotation = Quaternion.Slerp(startRot, Quaternion.Euler(goalRotation), rotationProgress / Tools.UnitRotationSpeed);
                }
                else
                {
                    actionFinished = true;
                    unit.facingDirection = goalDirection;
                }
            }
        }
    }
}
