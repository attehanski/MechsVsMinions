using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class MapSquare : MonoBehaviour
    {
        public enum Interactable
        {
            Interactable,
            Uninteractable,
            Passive
        }

        public bool enterable = true;
        public Vector2 materialTiling = new Vector2(1f, 1f);
        public Vector2 materialOffset;
        public GameObject editorIdentifier;

        [Header("Scenario dependent values")]
        public bool spawnMinionAtStart = false;
        public bool spawnsMinions = false;
        public bool playerSpawn = false;

        [Header("Runtime variables")]
        public Unit unit;
        public Dictionary<Tools.Direction, MapSquare> neighbours = new Dictionary<Tools.Direction, MapSquare>();
        
        [Header("Interaction references")]
        public Renderer highlight;
        public Interactable interactableState = Interactable.Passive;

        public Interactable interactable
        {
            get { return interactableState; }
            set {
                interactableState = value;
                ToggleHover(false);
            }
        }

        private void Awake()
        {
            if (editorIdentifier)
            {
                Destroy(editorIdentifier);
                editorIdentifier = null;
            }
        }

        private void Start()
        {
            InitMaterials();

            GetNeighbours();
            GenerateBorderSquares();            
            ToggleHover(false);

            if (spawnMinionAtStart)
                GameMaster.Instance.gearTracker.minionsList.Add(GameMaster.Instance.SpawnUnit(Prefabs.Instance.minion, this));
        }

        private void GetNeighbours()
        {
            foreach (Collider col in Physics.OverlapBox(transform.position, new Vector3(0.6f, 0.6f, 0.6f)))
            {
                MapSquare squareScr = col.GetComponent<MapSquare>();
                if (squareScr == null) continue;

                KeyValuePair<Tools.Direction, MapSquare> potentialNeighbour = CheckNeighbourState(this, squareScr);
                if (potentialNeighbour.Key != Tools.Direction.None && !neighbours.ContainsKey(potentialNeighbour.Key))
                    neighbours.Add(potentialNeighbour.Key, potentialNeighbour.Value);
            }
        }

        protected virtual void GenerateBorderSquares()
        {
            foreach(Tools.Direction direction in System.Enum.GetValues(typeof(Tools.Direction)))
                if(!neighbours.ContainsKey(direction) && direction != Tools.Direction.None)
                {
                    BorderSquare borderSquare = Instantiate(
                        Prefabs.Instance.borderSquare, 
                        transform.position + Tools.GetDirectionVector(direction), 
                        Quaternion.identity, 
                        transform.parent).GetComponent<BorderSquare>();
                    borderSquare.name = direction.ToString();
                    borderSquare.borderSquareCreator = this;
                    neighbours.Add(direction, borderSquare);
                }
        }

        public bool HasNeighbour(Tools.Direction direction)
        {
            if (neighbours.ContainsKey(direction))
                return true;
            return false;
        }

        public MapSquare GetNeighbour(Tools.Direction direction)
        {
            if (neighbours.ContainsKey(direction))
                return neighbours[direction];
            else
                return null;
        }

        public Tools.Direction GetNeighbourDirection(MapSquare neighbour)
        {
            foreach (KeyValuePair<Tools.Direction, MapSquare> kvp in neighbours)
                if (kvp.Value == neighbour)
                    return kvp.Key;

            return Tools.Direction.None;
        }

        public virtual void MoveToSquare(Unit movingUnit, Tools.Direction direction)
        {
            if (unit)
                movingUnit.Collide(unit);
            movingUnit.ChangeSquare(this); // NOTE: This probably needs to change for cases where we can't take up this space. Needs to be checked.
        }

        // NOTE: This could possibly be removed and just use simple bool instead
        public virtual bool CanMoveToSquare(Unit incomingUnit, Tools.Direction moveDirection)
        {
            return enterable;
        }

        public KeyValuePair<Tools.Direction, MapSquare> CheckNeighbourState(MapSquare origin, MapSquare other)
        {
            // If squares are not neighbours or they are the same one, return a none value.
            float distance = Vector3.Distance(origin.transform.position, other.transform.position);

            if (distance > 1.7f || distance < 0.9f)
                return new KeyValuePair<Tools.Direction, MapSquare>(Tools.Direction.None, other);

            Tools.Direction dir = Tools.GetDirection(origin, other);
            return new KeyValuePair<Tools.Direction, MapSquare>(dir, other);
        }

        public void ChangeHighlightColor(Color col)
        {
            highlight.material.color = col;
        }

        public void ToggleHover(bool isHovered)
        {
            switch(interactableState)
            {
                case Interactable.Passive:
                    highlight.enabled = isHovered;
                    ChangeHighlightColor(isHovered ? MapInput.Instance.hoverColor : MapInput.Instance.passiveColor);
                    break;

                case Interactable.Interactable:
                    highlight.enabled = true;
                    ChangeHighlightColor(isHovered ? MapInput.Instance.interactableHoveredColor : MapInput.Instance.interactableColor);
                    break;

                case Interactable.Uninteractable:
                    highlight.enabled = true;
                    ChangeHighlightColor(isHovered ? MapInput.Instance.uninteractableHoveredColor : MapInput.Instance.uninteractableColor);
                    break;
            }
        }

        protected virtual void InitMaterials()
        {
            Material highlightMat = highlight.material;    // This line is for making instances of the highlight materials for testing
            Material baseMaterial = GetComponent<Renderer>().material;
            baseMaterial.SetTextureOffset("_MainTex", materialOffset);
            baseMaterial.SetTextureScale("_MainTex", materialTiling);
        }
    }
}
