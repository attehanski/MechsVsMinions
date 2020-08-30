using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class UICardPanel : UIElement
    {
        public List<UICard> cards;

        public virtual void CardClicked(UICard card)
        {
            Debug.Log("Card " + card + " clicked!");
        }

        public virtual void AddCard(UICard card)
        {
            card.SetParentPanel(this);
            cards.Add(card);
            PlaceCard(card);
        }

        public virtual void RemoveCard(UICard card)
        {
            cards.Remove(card);
        }

        protected virtual void PlaceCard(UICard card)
        {
            card.rect.position = rect.position;
            card.rect.parent = rect;
        }

        public virtual void ClearCards()
        {
            for (int i = (cards.Count - 1); i >= 0; i--)
                Destroy(cards[i].gameObject);
            cards.Clear();
        }
    }
}