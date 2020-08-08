using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvM
{
    public class UIHighlight : MonoBehaviour
    {
        private Image image;

        private Image Image
        {
            get
            {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
            set { image = value; }
        }

        public enum HighlightState
        {
            Inactive,
            Available,
            Selected
        }

        public void ChangeState(HighlightState newState)
        {
            switch (newState)
            {
                case HighlightState.Inactive:
                    Image.enabled = false;
                    break;

                case HighlightState.Available:
                    Image.color = new Color(0f, 1f, 1f, 0.7f);
                    Image.enabled = true;
                    break;

                case HighlightState.Selected:
                    Image.color = new Color(1f, 0f, 0f, 0.5f);
                    Image.enabled = true;
                    break;

                default:
                    break;
            }
        }
    }
}