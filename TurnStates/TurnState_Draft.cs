using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class TurnState_Draft : TurnState
    {
        public List<CommandCard> draftCards = new List<CommandCard>();
        public int draftStage = 0;
        
        public TurnState_Draft()
        {
            stateName = "Draft";
            UIMaster.Instance.ChangeTurnState(stateName);
        }

        public override void StartState()
        {
            base.StartState();
            DrawDraftCards(5);

            // UI
            UIMaster.Instance.draftPanel.OpenDraftPanel(draftCards);
            UIMaster.Instance.commandLine.SetActive(true);
            UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Inactive);
        }

        public override void AdvanceState()
        {
            base.AdvanceState();

            GameMaster.Instance.currentTurnState = new TurnState_Players();
            UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Inactive);
            GameMaster.Instance.currentTurnState.StartState();
        }

        public override void UpdateState()
        {
            // If all players are ready, move onwards
            if (GameMaster.Instance.GetAllPlayersReady())
            {

            }
            // Else if current player has picked a card, move to next player
            else if (GameMaster.Instance.currentPlayer.ready)
            {
                GameMaster.Instance.NextPlayer();
            }
            base.UpdateState();
        }

        // TODO: Add functionality to add cards to the amount based on extra variables (Memory Core cards)
        public void DrawDraftCards(int amount)
        {
            draftCards.Clear(); // NOTE: Check if this is necessary
            for (int i = 0; i < amount; i++)
            {
                draftCards.Add(GameMaster.Instance.DrawCard(Card.Type.Command) as CommandCard);
            }
        }

        public override void AllPlayersReady()
        {
            // If everyone ready, end state
            if (draftStage == 2)
                GameMaster.Instance.FinishState();
            // If at slotting stage, end draft and enable ready button
            else if (draftStage == 1)
            {
                UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Ready);
                GameMaster.Instance.SetAllPlayersReady(false);
                draftStage = 2;
            }
            // If just finished picking cards, move to slotting
            else
            {
                UIMaster.Instance.draftPanel.EndDraft();
                UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Scrap);
                GameMaster.Instance.SetAllPlayersReady(false);
                draftStage = 1;
                GameMaster.Instance.UpdateUIState();
            }
        }

        public virtual void CardPicked(CommandCard card)
        {
            draftCards.Remove(card);
            GameMaster.Instance.currentPlayer.hand.Add(card);
            SetCurrentCard(card);
            GameMaster.Instance.currentPlayer.ready = true;
        }

        public void SetCurrentCard(CommandCard card)
        {
            GameMaster.Instance.currentPlayer.currentCard = card;
        }

        public virtual void CardSlotted()
        {
            GameMaster.Instance.currentPlayer.ready = true;
        }
    }
}