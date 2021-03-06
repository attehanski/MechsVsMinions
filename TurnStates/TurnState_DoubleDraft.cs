﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class TurnState_DoubleDraft : TurnState_Draft
    {
        public TurnState_DoubleDraft()
        {
            stateName = "First Draft - Pick 2";
            UIMaster.Instance.ChangeTurnState(stateName);
        }

        public override void StartState()
        {
            GameMaster.Instance.UpdateUIState();
            DrawDraftCards(10);
            UIMaster.Instance.draftPanel.OpenDraftPanel(draftCards);
            UIMaster.Instance.commandLine.SetActive(true);
            draftStage = -1; // NOTE: This is a hack to make sure double draft is in same numerical stage as normal draft when slotting
        }

        public override void UpdateState()
        {
            // If current player has picked a card, move to next player
            if (GameMaster.Instance.currentPlayer.ready)
            {
                GameMaster.Instance.NextPlayer();
            }
            base.UpdateState();
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
            else if (draftStage == 0)
            {
                // DATA
                GameMaster.Instance.SetAllPlayersReady(false);
                draftStage = 1;
                GameMaster.Instance.UpdateUIState();
                foreach (Card card in draftCards)
                    GameMaster.Instance.commandCardDeck.DiscardCard(card as CommandCard);

                // UI
                UIMaster.Instance.draftPanel.EndDraft();
                UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Scrap);
            }
            // Players have picked their first card
            else
            {
                GameMaster.Instance.SetAllPlayersReady(false);
                draftStage = 0;
            }
        }

        public override void CardSlotted()
        {
            if (GameMaster.Instance.currentPlayer.hand.Count == 0)
                GameMaster.Instance.currentPlayer.ready = true;
            else
                UIMaster.Instance.handPanel.CardClicked(UIMaster.Instance.handPanel.cards[0]);
        }
    }
}