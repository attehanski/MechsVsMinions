using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MvM
{
    [RequireComponent(typeof(Button))]
    public class UICard : UIElement, IPointerDownHandler, IPointerUpHandler
    {
        public Text text;
        public Image BG;
        [HideInInspector]
        public Button button;
        [HideInInspector]
        public Canvas canvas;

        [Header("Data")]
        public Card cardData; // NOTE: This is not required in the UI code, just for testing purposes

        private UICardPanel parentPanel;
        private UICard cardCloseup;

        protected override void Awake()
        {
            base.Awake();
            button = GetComponent<Button>();
            canvas = GetComponent<Canvas>();
        }

        public void InitCard(Card card)
        {
            cardData = card;
            if (card.textureAsset != null)
                BG.sprite = card.textureAsset;

            if (card is DamageCard || card.textureAsset == null)
                text.text = cardData.text;

            if (card is CommandCard)
                text.text = "";
        }

        public void CardClicked()
        {
            if (parentPanel)
                parentPanel.CardClicked(this);
        }

        public override void SetHighlightState(UIHighlight.HighlightState highlightState)
        {
            base.SetHighlightState(highlightState);
            canvas.overrideSorting = true;
            if (highlightState == UIHighlight.HighlightState.Available)
                canvas.sortingOrder = 2;
            else
                canvas.sortingOrder = 1;
        }

        public void SetParentPanel(UICardPanel newParent)
        {
            parentPanel = newParent;
            rect.parent = newParent.rect;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                cardCloseup = CreateCloseup();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (cardCloseup)
                    Destroy(cardCloseup.gameObject);
            }
        }

        private UICard CreateCloseup()
        {
            UICard closeup = Instantiate(Prefabs.Instance.card, UIMaster.Instance.transform).GetComponent<UICard>();
            closeup.InitCard(cardData);
            closeup.rect.localScale *= 3;
            closeup.rect.position = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
            closeup.canvas.sortingOrder = 4;
            closeup.button.interactable = false;
            return closeup;
        }
    }
}