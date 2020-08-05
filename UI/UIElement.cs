using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class UIElement : MonoBehaviour
    {
        public GameObject highlight;

        public virtual void ToggleHighlight(bool on)
        {
            highlight.SetActive(on);
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