using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class UIElement : MonoBehaviour
    {
        public UIHighlight highlight;

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