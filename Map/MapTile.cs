using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class MapTile : MonoBehaviour
    {
        [System.Serializable]
        public class MapSquareMatrix
        {
            public string rowName;
            public MapSquare[] squares;
        }

        public enum Type
        {
            School,
            MagmaChamber
        }

        public Type type;
        public MapSquareMatrix[] tileSquares = new MapSquareMatrix[6];

        protected MapSquare[] squares;

        protected virtual void Awake()
        {
            SetSquareValues();
        }

        protected virtual void SetSquareValues()
        {
            for (int i = 0; i < tileSquares.Length; i++)
                for (int j = 0; j < tileSquares[i].squares.Length; j++)
                    if (tileSquares[i].squares[j] != null)
                        tileSquares[i].squares[j].materialOffset = new Vector2((5 - j) / 6f, i / 6f);
            squares = GetComponentsInChildren<MapSquare>();
        }
    }
}
