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
        public bool commandLineFinished = false;
        
        private Card cardBeingExecuted;

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

        #region Commandline Execution
        public IEnumerator ExecuteCurrentCommandLine(Player player, bool reverseOrder = false)
        {
            commandLineFinished = false;
            ToggleCharacterHighlight(player, true);

            int end = reverseOrder ? -1 : 6;
            for (int i = reverseOrder ? 5 : 0; (!reverseOrder && i < end) || (reverseOrder && i > end); i = reverseOrder ? i - 1 : i + 1)
            {
                yield return ExecuteCardSlot(i, player);
            }
            commandLineFinished = true;
            ToggleCharacterHighlight(player, false);
            yield return null;
        }

        private IEnumerator ExecuteCardSlot(int slotIndex, Player player)
        {
            GameMaster.Instance.interactedSquare = null;

            cardBeingExecuted = GetTopCard(slotIndex);
            if (cardBeingExecuted != null)
            {
                UIMaster.Instance.cardSlots[slotIndex].SetHighlightState(UIHighlight.HighlightState.Available);
                cardBeingExecuted.InitializeCardExecution(player.character); // NOTE: hardcoded for players, requires recoding if used for other units!

                while (cardBeingExecuted.cardState != Card.CardState.Finished)
                {
                    if (cardBeingExecuted.cardState != Card.CardState.NoInputRequired)
                    {
                        if (ShowInputOptions(cardBeingExecuted))
                        {
                            yield return WaitForInput();
                        }
                        else
                            break;
                    }
                    yield return DoActions(player);
                    cardBeingExecuted.UpdateCardState();
                }
                UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Ready);
                yield return WaitForPlayerReady(player);

                UIMaster.Instance.cardSlots[slotIndex].SetHighlightState(UIHighlight.HighlightState.Inactive);
                cardBeingExecuted = null;
            }
            else
                yield break;
        }

        private void ToggleCharacterHighlight(Player player, bool highlightOn)
        {
            player.character.ToggleHighlight(highlightOn);
        }

        private bool ShowInputOptions(Card card)
        {
            Dictionary<MapSquare, MapSquare.Interactable> inputSquares = card.GetValidInputSquares();
            if (!InputOptionsAvailable(inputSquares.Values))
            {
                card.NoViableInputOptions();
                return false;
            }
            else
            {
                MapInput.Instance.SetInteractables(inputSquares);
                return true;
            }
        }

        private bool InputOptionsAvailable(Dictionary<MapSquare, MapSquare.Interactable>.ValueCollection options)
        {
            foreach (MapSquare.Interactable option in options)
                if (option == MapSquare.Interactable.ActiveChoice || option == MapSquare.Interactable.NonfinalChoice)
                    return true;
            return false;
        }

        private void InputReceived(MapSquare inputSquare)
        {
            MapInput.Instance.ClearInteractables();
            cardBeingExecuted.Input(inputSquare);
            GameMaster.Instance.interactedSquare = null;
        }

        private IEnumerator WaitForInput()
        {
            while (!GameMaster.Instance.interactedSquare)
            {
                if (GameMaster.Instance.currentPlayer.ready)
                {
                    ExitCardExecutionEarly();
                    yield break;
                }
                yield return null;
            }

            InputReceived(GameMaster.Instance.interactedSquare);
        }

        private void ExitCardExecutionEarly()
        {
            MapInput.Instance.ClearInteractables();
        }

        private IEnumerator DoActions(Player player)
        {
            cardBeingExecuted.ExecuteCard();
            while (player.character.actionsInProgress)
                yield return null;
        }

        private IEnumerator WaitForPlayerReady(Player player)
        {
            while (!player.ready)
            {
                yield return null;
            }
            player.ready = false;
        }
        #endregion
    }
}
