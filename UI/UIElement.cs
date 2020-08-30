using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    [RequireComponent(typeof(RectTransform))]
    public class UIElement : MonoBehaviour
    {
        [Header("UI Object references")]
        public UIHighlight highlight;

        [HideInInspector]
        public RectTransform rect;

        protected virtual void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        public virtual void SetHighlightState(UIHighlight.HighlightState highlightState)
        {
            if (highlight) highlight.ChangeState(highlightState);
            else Debug.Log("Object " + name + " highlight is: " + highlight);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}