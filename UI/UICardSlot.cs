using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvM
{
    public class UICardSlot : UICardPanel
    {
        public int index;

        public void CardSlotClicked()
        {
            GameMaster.Instance.CardSlotInteracted(this);
        }

        public void SlotCard(UICard card)
        {
            // TODO: Move discarding check earlier in the slotting handling
            if (cards.Count > 0)
            {
                if ((card.cardData as CommandCard).cardColor != (cards[cards.Count - 1].cardData as CommandCard).cardColor)
                    ClearCards();
                else if (cards.Count > 2)
                    RemoveCard(cards[0]);
            }
            AddCard(card);
        }

        public void SlotCard(Card card)
        {
            // TODO: Change this check to come from the code that discards the card in the data.
            if (cards.Count > 0 && card is DamageCard && cards[cards.Count - 1].cardData is DamageCard)
                RemoveCard(cards[cards.Count - 1]);
            AddCard(UIMaster.InstantiateCard(card, rect));
        }

        public void RepairSlot()
        {
            if (cards[cards.Count - 1].cardData.type == Card.Type.Damage)
                RemoveCard(cards[cards.Count - 1]);
            else
                Debug.LogError("Attempted to repair an undamaged slot!");
        }
        
        public override void CardClicked(UICard card)
        {
            CardSlotClicked();
            Debug.Log("Slotted card clicked: " + card.cardData.text);
        }

        public override void AddCard(UICard card)
        {
            base.AddCard(card);
            card.SetHighlightState(UIHighlight.HighlightState.Inactive);
            UpdateCardValues();
        }

        private void UpdateCardValues()
        {
            foreach (UICard slottedCard in cards)
            {
                PlaceCard(slottedCard);
                slottedCard.button.interactable = cards.IndexOf(slottedCard) == cards.Count - 1;
                slottedCard.canvas.sortingOrder = cards.IndexOf(slottedCard) + 1;
            }
        }

        public override void RemoveCard(UICard card)
        {
            base.RemoveCard(card);
            Destroy(card.gameObject);
            UpdateCardValues();
        }

        protected override void PlaceCard(UICard card)
        {
            card.rect.position = transform.position + new Vector3(0f, 15 - 15 * cards.IndexOf(card), 0f);
        }

        public void SwapCards(UICardSlot otherSlot)
        {
            List<UICard> temp = cards;
            cards = otherSlot.cards;
            otherSlot.cards = temp;

            ReplaceCards();
            otherSlot.ReplaceCards();
        }

        public void ReplaceCards()
        {
            foreach (UICard card in cards)
            {
                PlaceCard(card);
                card.SetParentPanel(this);
            }
        }
    }
}
