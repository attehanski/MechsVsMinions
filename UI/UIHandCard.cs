using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvM
{
    public class UIHandCard : UIDraftCard
    {
        public Button button;
        private UIHand hand;

        public void Awake()
        {
            hand = transform.parent.GetComponent<UIHand>();
            button = GetComponent<Button>();
        }

        public override void SelectChoice()
        {
            hand.UnHighlightCards();
            GameMaster.Instance.localPlayer.currentCard = draftCard;
            SetHighlightState(UIHighlight.HighlightState.Available);
        }

        public override void SetHighlightState(UIHighlight.HighlightState highlightState)
        {
            base.SetHighlightState(highlightState);
            Canvas canvas = GetComponent<Canvas>();
            canvas.overrideSorting = true;
            if (highlightState == UIHighlight.HighlightState.Available)
            {
                transform.position = new Vector3(transform.position.x, hand.transform.position.y + 20f, 0); // NOTE: This is not easily scalable to different resolutions
                canvas.sortingOrder = 2;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, hand.transform.position.y - 10f, 0); // NOTE: This is not easily scalable to different resolutions
                canvas.sortingOrder = 1;
            }
        }

    }
}