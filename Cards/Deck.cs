using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    [System.Serializable]
    public class Deck<T> where T : Card
    {
        public CardStack<T> deck = new CardStack<T>();
        public CardStack<T> discard = new CardStack<T>();

        /// <summary>
        /// Shuffles the deck using the Fisher-Yates algorithm.
        /// </summary>
        public void ShuffleDeck()
        {
            var cards = deck.ToArray();
            for (int i = cards.Length - 1; i > 0; i--)
            {
                int random = Random.Range(0, i);
                var temp = cards[random];
                cards[random] = cards[i];
                cards[i] = temp;
            }

            CardStack<T> newStack = new CardStack<T>();
            foreach (var card in cards)
            {
                newStack.AddTop(card);
            }

            deck = newStack;
        }

        public (CardStack<T>, CardStack<T>) ResetDeck(CardStack<T> deck, CardStack<T> discard)
        {
            deck = new CardStack<T>(discard);
            discard.Clear();
            ShuffleDeck();

            return (deck, discard);
        }

        public void AddCardToDeck(T card)
        {
            deck.AddTop(card);
        }

        public T DrawCard()
        {
            T drawnCard = null;

            if (deck.Count < 1)
                (deck, discard) = ResetDeck(deck, discard);
            drawnCard = deck.PopTop();

            return drawnCard;
        }

        public void DiscardCard(T card)
        {
            discard.AddTop(card);
        }

        public void Clear()
        {
            deck = new CardStack<T>();
            discard = new CardStack<T>();
        }
    }
}