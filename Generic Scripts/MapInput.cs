using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class MapInput : Singleton<MapInput>
    {
        [System.Serializable]
        public class MapSquareColors
        {
            public Color passiveColor;
            public Color hoverColor;
            public Color interactableColor;
            public Color interactableHoveredColor;
            public Color nonfinalColor;
            public Color nonfinalHoveredColor;
            public Color uninteractableColor;
            public Color uninteractableHoveredColor;
        }

        public Vector3 cameraBounds;
        public float cameraMoveMultiplier;
        public LayerMask raycastMask;

        [Header("Highlight colors")]
        public MapSquareColors colors;

        private Ray ray;
        private RaycastHit hit;
        private Camera cam;
        private MapSquare hoveredSquare;
        private MapSquare target;

        private float inputMinDelay = 0.45f;
        private float currentInputDelay = 0f;

        private float mapMoveHoldTime = 0.2f;
        private float buttonHoldTime = 0f;

        void Start()
        {
            cam = GetComponent<Camera>();
        }
        
        void Update()
        {
#if UNITY_ANDROID
             // Different functionality for mobile?
#else

            if (Input.GetButton("Fire1"))
            {
                buttonHoldTime += Time.deltaTime;

                if (buttonHoldTime > mapMoveHoldTime)
                    MoveCamera();
            }

            ray = cam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 500f, raycastMask);
            if (!hit.collider)
            {
                if (hoveredSquare)
                    hoveredSquare.ToggleHover(false);
                hoveredSquare = null;
            }
            else
            {
                target = hit.collider.GetComponent<MapSquare>();
                if (target)
                {
                    if (hoveredSquare)
                        hoveredSquare.ToggleHover(false);

                    target.ToggleHover(true);
                    hoveredSquare = target;
                }

                if (Input.GetButtonUp("Fire1"))
                {
                    if (target &&
                        (target.interactable == MapSquare.Interactable.ActiveChoice || target.interactable == MapSquare.Interactable.NonfinalChoice) &&
                        buttonHoldTime < mapMoveHoldTime &&
                        currentInputDelay > inputMinDelay)
                    {
                        SquareInteracted(target);
                        currentInputDelay = 0f;
                    }

                    buttonHoldTime = 0f;
                }
            }

            currentInputDelay += Time.deltaTime;
#endif
        }

        public void SquareInteracted(MapSquare square)
        {
            target.interactable = MapSquare.Interactable.Passive;
            GameMaster.Instance.MapSquareInteracted(square);
        }

        private void MoveCamera()
        {
            Vector3 camTranslation = cam.transform.position;
            camTranslation += new Vector3(-1 * Input.GetAxis("Mouse X") * cameraMoveMultiplier, 0f, -1 * Input.GetAxis("Mouse Y") * cameraMoveMultiplier);
            camTranslation = LockCameraPosition(camTranslation);
            cam.transform.position = camTranslation;
        }

        private Vector3 LockCameraPosition(Vector3 currentPos)
        {
            if (currentPos.x > cameraBounds.x)
                currentPos.x = cameraBounds.x;
            else if (currentPos.x < -cameraBounds.x)
                currentPos.x = -cameraBounds.x;

            if (currentPos.z > cameraBounds.z)
                currentPos.z = cameraBounds.z;
            else if (currentPos.z < -cameraBounds.z)
                currentPos.z = -cameraBounds.z;

            return currentPos;
        }

        #region Map highlighting
        public void SetInteractables(Dictionary<MapSquare, MapSquare.Interactable> interactableSquares)
        {
            foreach (var square in interactableSquares)
                square.Key.interactable = square.Value;
        }

        public void ClearInteractables()
        {
            foreach (var square in FindObjectsOfType<MapSquare>())
                square.interactable = MapSquare.Interactable.Passive;
        }
        #endregion
    }
}