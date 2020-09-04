using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvM
{
    public class UIPanelMover : MonoBehaviour
    {
        [Header("Moving panel variables")]
        public UIElement movingPanel;
        public Vector3 panelMovePosition;
        public float panelMoveTime = 1f;
        [Header("Button variables")]
        public RectTransform arrow;

        private Vector3 panelStartPosition;
        private bool panelMoved = false;
        private bool animationRunning = false;
        private float panelMoveElapsed = 0f;

        private void Start()
        {
            panelStartPosition = movingPanel.rect.anchoredPosition;
        }

        public void MovePanel()
        {
            if (animationRunning) return;
            if (panelMoved)
                StartCoroutine(AnimatePanelMove(panelMovePosition, panelStartPosition));
            else
                StartCoroutine(AnimatePanelMove(panelStartPosition, panelMovePosition));
            panelMoved = !panelMoved;
        }

        private IEnumerator AnimatePanelMove(Vector3 oldPanelPosition, Vector3 newPanelPosition)
        {
            animationRunning = true;
            panelMoveElapsed = 0f;
            while(panelMoveElapsed < panelMoveTime)
            {
                movingPanel.rect.anchoredPosition = Vector3.Lerp(oldPanelPosition, newPanelPosition, panelMoveElapsed / panelMoveTime);
                panelMoveElapsed += Time.deltaTime;
                yield return null;
            }
            movingPanel.rect.anchoredPosition = newPanelPosition;
            arrow.localEulerAngles = new Vector3(arrow.localEulerAngles.x, arrow.localEulerAngles.y, arrow.localEulerAngles.z + 180);
            animationRunning = false;
        }
    }
}