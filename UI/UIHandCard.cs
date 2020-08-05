using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class UIHandCard : UIDraftCard
    {
        private UIHand hand;

        public void Awake()
        {
            hand = transform.parent.GetComponent<UIHand>();
        }

        public override void SelectChoice()
        {
            hand.UnHighlightCards();
            GameMaster.Instance.localPlayer.currentCard = draftCard;
            ToggleHighlight(true);
        }

        public override void ToggleHighlight(bool on)
        {
            base.ToggleHighlight(on);
            if (on)
            {
                transform.position = new Vector3(transform.position.x, hand.transform.position.y + 20f, 0); // NOTE: This is not easily scalable to different resolutions
                GetComponent<Canvas>().sortingOrder = 2;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, hand.transform.position.y - 10f, 0); // NOTE: This is not easily scalable to different resolutions
                GetComponent<Canvas>().sortingOrder = 1;
            }
        }

    }
}