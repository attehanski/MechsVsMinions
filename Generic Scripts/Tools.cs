using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    /// <summary>
    /// Generic class for static values, functions enums etc.
    /// </summary>
    public class Tools
    {
        public static float UnitSpeed = 0.25f;
        public static float UnitRotationSpeed = 0.25f;

        public enum Direction
        {
            NorthWest,
            North,
            NorthEast,
            West,
            East,
            SouthWest,
            South,
            SouthEast,
            None
        }

        public enum Facing
        {
            Forward,
            ForwardLeft,
            Left,
            ForwardRight,
            Right,
            BackLeft,
            BackRight,
            Back
        }

        public enum Color
        {
            None,
            Red,
            Green,
            Blue,
            Yellow
        }

        /// <summary>
        /// 
        /// NOTE: Locked to using a z-axis normal vector!
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float CalculateAngle(Vector3 a, Vector3 b)
        {
            float angle = Vector3.Angle(new Vector3(0f, 0f, 1f), b - a);
            return Mathf.Round(angle * Mathf.Sign((b - a).x));
        }

        public static Direction GetDirection(MapSquare a, MapSquare b)
        {
            float angle = CalculateAngle(a.transform.position, b.transform.position);

            switch (angle)
            {
                case 0:
                    return Direction.North;

                case 45:
                    return Direction.NorthEast;

                case 90:
                    return Direction.East;

                case 135:
                    return Direction.SouthEast;

                case float n when n == 180 || n == -180:
                    return Direction.South;

                case -135:
                    return Direction.SouthWest;

                case -90:
                    return Direction.West;

                case -45:
                    return Direction.NorthWest;

                default:
                    Debug.Log("GetDirection returning none for angle: " + angle);
                    return Direction.None;
            }
        }

        public static Vector3 GetDirectionVector(Direction direction)
        {
            switch (direction)
            {
                case Direction.NorthWest:
                    return new Vector3(-1f, 0f, 1f);

                case Direction.North:
                    return new Vector3(0f, 0f, 1f);

                case Direction.NorthEast:
                    return new Vector3(1f, 0f, 1f);

                case Direction.East:
                    return new Vector3(1f, 0f, 0f);

                case Direction.SouthEast:
                    return new Vector3(1f, 0f, -1f);

                case Direction.South:
                    return new Vector3(0f, 0f, -1f);

                case Direction.SouthWest:
                    return new Vector3(-1f, 0f, -1f);

                case Direction.West:
                    return new Vector3(-1f, 0f, 0f);

                default:
                    return Vector3.zero;
            }

        }

        public static Direction GetDirectionFromFacing(Facing facing, Direction currentDirection)
        {
            if ((facing == Facing.Forward && currentDirection == Direction.North) ||
                (facing == Facing.Right && currentDirection == Direction.West) ||
                (facing == Facing.Left && currentDirection == Direction.East) ||
                (facing == Facing.Back && currentDirection == Direction.South))
                return Direction.North;
            if ((facing == Facing.Forward && currentDirection == Direction.South) ||
                (facing == Facing.Right && currentDirection == Direction.East) ||
                (facing == Facing.Left && currentDirection == Direction.West) ||
                (facing == Facing.Back && currentDirection == Direction.North))
                return Direction.South;
            if ((facing == Facing.Forward && currentDirection == Direction.East) ||
                (facing == Facing.Right && currentDirection == Direction.North) ||
                (facing == Facing.Left && currentDirection == Direction.South) ||
                (facing == Facing.Back && currentDirection == Direction.West))
                return Direction.East;
            if ((facing == Facing.Forward && currentDirection == Direction.West) ||
                (facing == Facing.Right && currentDirection == Direction.South) ||
                (facing == Facing.Left && currentDirection == Direction.North) ||
                (facing == Facing.Back && currentDirection == Direction.East))
                return Direction.West;

            Debug.LogError("Direction from facing returning none!");
            return Direction.None;
        }

        public static Vector3 GetRotation(Direction newFacing)
        {
            switch (newFacing)
            {
                case Direction.North:
                    return new Vector3(0f, -90f, 0f);

                case Direction.East:
                    return new Vector3(0f, 0f, 0f);

                case Direction.South:
                    return new Vector3(0f, 90f, 0f);

                case Direction.West:
                    return new Vector3(0f, 180f, 0f);

                default:
                    return Vector3.zero;
            }
        }

        public static UnityEngine.Color GetColor(Color color)
        {
            switch (color)
            {
                case Color.Blue:
                    return UnityEngine.Color.blue;

                case Color.Green:
                    return UnityEngine.Color.green;

                case Color.Red:
                    return UnityEngine.Color.red;

                case Color.Yellow:
                    return UnityEngine.Color.yellow;

                default:
                    return UnityEngine.Color.black;
            }
        }

        public static bool UnitIsEnemy(Unit unit)
        {
            if (unit == null) return false;
            return unit is Minion || unit is CrystalDestroyable; /*|| neighbour.Value.unit is Boss*/
        }
    }
}
