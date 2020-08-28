using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class UIHand : UIElement
    {
        public List<UIHandCard> cards = new List<UIHandCard>();

        public void AddCard(Card card)
        {
            gameObject.SetActive(true);
            UnHighlightCards();

            float xPos = -30 + cards.Count * 30;
            float yPos = -10;

            UIHandCard handCard = Instantiate(Prefabs.Instance.handCard, transform.position + new Vector3(xPos, yPos, 0f), Quaternion.identity, transform).GetComponent<UIHandCard>();
            handCard.InitCard(card);
            cards.Add(handCard);
            handCard.SelectChoice();
        }

        public void RemoveCard(Card card)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].draftCard == card)
                {
                    Destroy(cards[i].gameObject);
                    cards.Remove(cards[i]);

                    if (cards.Count > 0)
                        cards[0].GetComponent<Canvas>().sortingOrder = 2;
                    else
                        gameObject.SetActive(false);

                    break;
                }
            }
        }

        public void UnHighlightCards()
        {
            foreach (UIHandCard card in cards)
                card.SetHighlightState(UIHighlight.HighlightState.Inactive);
        }

        public void SetHandLocked(bool locked)
        {
            foreach (UIHandCard card in cards)
                card.button.interactable = !locked;
        }

        public void ClearHand()
        {
            int lastIndex = cards.Count - 1;
            for (int i = 0; i < cards.Count; i++)
            {
                Destroy(cards[lastIndex - i].gameObject);
            }
            cards.Clear();
        }
    }
}