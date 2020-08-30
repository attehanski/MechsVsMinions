using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class UIHand : UICardPanel
    {
        public UICard currentCard;

        public void RemoveCard(Card card)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].cardData == card)
                {
                    Destroy(cards[i].gameObject);
                    cards.Remove(cards[i]);
                    break;
                }
            }

            if (cards.Count > 0)
                cards[0].canvas.sortingOrder = 2;
            else
                gameObject.SetActive(false);
        }

        public void UnHighlightCards()
        {
            foreach (UICard card in cards)
            {
                card.SetHighlightState(UIHighlight.HighlightState.Inactive);
                card.rect.position = new Vector3(card.rect.position.x, transform.position.y - 10f, 0f);
            }
        }

        public void SetHandLocked(bool locked)
        {
            foreach (UICard card in cards)
                card.button.interactable = !locked;
        }

        public override void CardClicked(UICard card)
        {
            SelectCard(card);
            (GameMaster.Instance.currentTurnState as TurnState_Draft).SetCurrentCard(card.cardData as CommandCard);
        }

        public void SlotCurrentCard(UICardSlot slot)
        {
            cards.Remove(currentCard);
            slot.SlotCard(currentCard);
            if (cards.Count < 1)
                Hide();
        }

        public override void AddCard(UICard card)
        {
            gameObject.SetActive(true);
            UnHighlightCards();
            base.AddCard(card);
            SelectCard(card);
        }

        protected override void PlaceCard(UICard card)
        {
            float xPos = -50 + cards.Count * 30;
            float yPos = -10;
            card.rect.position = transform.position + new Vector3(xPos, yPos, 0f);
        }

        private void SelectCard(UICard card)
        {
            UnHighlightCards();
            card.SetHighlightState(UIHighlight.HighlightState.Available);
            card.rect.position = new Vector3(card.rect.position.x, transform.position.y + 0f, 0f);
            currentCard = card;
        }
    }
}