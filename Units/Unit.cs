using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Unit : MonoBehaviour
    {
        public GameObject model;
        public GameObject highlight;
        public bool canPush;
        public bool canBePushed;

        [Header("Runtime variables")]
        public Tools.Direction facingDirection = Tools.Direction.East;
        public MapSquare mapSquare;
        public bool actionsInProgress = false;

        public Stack<Action> actionStack = new Stack<Action>();
        private Action currentAction = null;
        
        public void ExecuteActions()
        {
            StartCoroutine(ExecuteStack());
        }

        /// <summary>
        /// Executes stack while there are actions in it.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ExecuteStack()
        {
            actionsInProgress = true;
            while (actionStack.Count > 0 || currentAction != null)
            {
                currentAction = actionStack.Pop();
                // While top item is not finished, update it
                while (!currentAction.actionFinished)
                {
                    currentAction.UpdateState();
                    yield return null;
                }

                // After top item is finished, pop it
                currentAction = null;
                yield return null;
            }
            actionsInProgress = false;
            yield return null;
        }

        #region Movement functions

        /// <summary>
        /// Adds a move command towards given direction to the stack.
        /// </summary>
        /// <param name="direction"></param>
        public virtual void Move(Tools.Direction direction)
        {
            actionStack.Push(new MoveAction(this, direction));
        }

        public virtual void Move(Tools.Facing moveTowards)
        {
            actionStack.Push(new MoveAction(this, Tools.GetDirectionFromFacing(moveTowards, facingDirection)));
        }

        public virtual void AttemptMove(Tools.Direction direction)
        {
            if (CanMove(direction))
                ChangeSquare(mapSquare.GetNeighbour(direction), direction);
            else
                mapSquare.GetNeighbour(direction).FailedEnteringSquare(this);
        }

        public virtual bool CanMove(Tools.Direction direction)
        {
            MapSquare targetSquare = mapSquare.GetNeighbour(direction);
            return targetSquare != null && targetSquare.CanEnterSquare(this, direction);
        }

        public virtual bool CanMove(Tools.Facing moveFacing)
        {
            Tools.Direction moveDirection = Tools.GetDirectionFromFacing(moveFacing, facingDirection);
            return CanMove(moveDirection);
        }

        public virtual bool CanBeReplaced(Unit replacingUnit, Tools.Direction direction)
        {
            return false;
        }

        public virtual void ChangeSquare(MapSquare newSquare, Tools.Direction moveDirection)
        {
            newSquare.MoveToSquare(this, moveDirection);

            mapSquare.unit = null;
            mapSquare = newSquare;
            newSquare.unit = this;
        }

        public virtual void Collide(Unit collisionTarget)
        {
            // Handle whatever happens to this unit
        }

        public virtual void IsCollided(Unit collidingUnit)
        {
            mapSquare.unit = collidingUnit;
        }

        #endregion

        /// <summary>
        /// Adds a rotate command towards a given direction to the stack.
        /// </summary>
        /// <param name="newFacing"></param>
        public virtual void Turn(Tools.Direction newFacing)
        {
            actionStack.Push(new TurnAction(this, newFacing));
        }

        public virtual void Turn(Tools.Facing turnTowards)
        {
            actionStack.Push(new TurnAction(this, Tools.GetDirectionFromFacing(turnTowards, facingDirection)));
        }

        public virtual void TakeDamage(Tools.Color damageColor = Tools.Color.None)
        {
            //Debug.Log("Unit " + name + " took damage!");
        }

        public virtual void ToggleHighlight(bool toggle)
        {
            highlight.SetActive(toggle);
        }
    }
}
