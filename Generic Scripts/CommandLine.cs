using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvM
{
    [System.Serializable]
    public class CommandLine
    {
        public CardStack<Card>[] cards = new CardStack<Card>[6];

        public CommandLine()
        {
            for (int i = 0; i < cards.Length; i++)
                cards[i] = new CardStack<Card>();
        }

        public bool SlotCard(int index, Card card)
        {
            // If for some reason trying to slot a card out of the command line, return
            if (index < 0 || index >= cards.Length)
                return false;

            bool success = false;
            if (card is CommandCard)
                success = SlotCommandCard(index, card as CommandCard);
            else if (card is DamageCard)
                success = SlotDamageCard(index, card as DamageCard);
            else
            {
                Debug.LogError("Trying to slot a card that is neither damage or command!");
                return false;
            }

            return success;
        }

        private bool SlotCommandCard(int index, CommandCard card)
        {
            // If there are already cards in the stack
            if (cards[index].Count > 0)
            {
                if (!(cards[index].PeekTop() is CommandCard))
                    return false;

                CommandCard oldTopCard = cards[index].PeekTop() as CommandCard;
                // If card is same color, add new one to the stack.
                if (oldTopCard.cardColor == card.cardColor)
                {
                    // If stack is full remove bottom card from stack.
                    if (cards[index].Count == 3)
                        GameMaster.Instance.DiscardCard(cards[index].PopBottom());

                    card.level = Mathf.Min(3, oldTopCard.level + 1);
                }
                // If card is different color, discard old stack.
                else
                {
                    int stackSize = cards[index].Count;
                    for (int i = 0; i < stackSize; i++)
                        GameMaster.Instance.DiscardCard(cards[index].PopTop());
                }
            }

            cards[index].AddTop(card);
            return true;
        }

        private bool SlotDamageCard(int index, DamageCard card)
        {
            // If slot already has a damage card, discard the old one
            if (cards[index].Count > 0)
            {
                if (cards[index].PeekTop() is DamageCard)
                    GameMaster.Instance.DiscardCard(cards[index].PopTop());
            }

            cards[index].AddTop(card);
            return true;
        }

        public bool CanSlotCard(int index, Card card)
        {
            // If the slot has a damage card, a new card cannot be slotted
            if (cards[index].Count > 0 && cards[index].PeekTop() is DamageCard)
                return false;

            return true;
        }

        public Card GetTopCard(int index)
        {
            if (cards[index].Count > 0)
                return cards[index].PeekTop();
            else
                return null;
        }
    }
}
