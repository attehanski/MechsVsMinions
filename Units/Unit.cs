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
            while (actionStack.Count > 0)
            {
                // While top item is not finished, update it
                while (!actionStack.Peek().actionFinished)
                {
                    actionStack.Peek().UpdateState();
                    yield return null;
                }

                // After top item is finished, pop it
                actionStack.Pop();
                yield return null;
            }
            actionsInProgress = false;
            yield return null;
        }

        /// <summary>
        /// Adds a move command towards given direction to the stack.
        /// </summary>
        /// <param name="direction"></param>
        public virtual void Move(Tools.Direction direction)
        {
            if (CanMove(direction))
                actionStack.Push(new MoveAction(this, direction));
        }

        public virtual void Move(Tools.Facing moveTowards)
        {
            if (CanMove(moveTowards))
                actionStack.Push(new MoveAction(this, Tools.GetDirectionFromFacing(moveTowards, facingDirection)));
        }

        public virtual bool CanMove(Tools.Direction direction)
        {
            MapSquare targetSquare = mapSquare.GetNeighbour(direction);

            // Check if target square is movable
            bool squareMovable = targetSquare != null && targetSquare.CanMoveToSquare(this, direction);
            // Check if target square has a unit
            bool squareIsFree = targetSquare.unit == null;

            return squareMovable && squareIsFree;
        }

        public virtual bool CanMove(Tools.Facing moveFacing)
        {
            Tools.Direction moveDirection = Tools.GetDirectionFromFacing(moveFacing, facingDirection);
            return CanMove(moveDirection);
        }

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

        public virtual void ChangeSquare(MapSquare newSquare)
        {
            mapSquare.unit = null;
            mapSquare = newSquare;
            newSquare.unit = this;
        }

        public virtual void Collide(Unit collisionTarget)
        {
            // Handle whatever happens to this unit
            collisionTarget.IsCollided(this);
        }

        public virtual void IsCollided(Unit collidingUnit)
        {
            mapSquare.unit = collidingUnit;
        }

        public virtual void ToggleHighlight(bool toggle)
        {
            highlight.SetActive(toggle);
        }
    }
}
