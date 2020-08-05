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
            UIMaster.Instance.draftPanel.OpenDraftPanel();
            UIMaster.Instance.commandLine.SetActive(true);
            UIMaster.Instance.UpdateContinueButton(false);
        }

        public override void AdvanceState()
        {
            base.AdvanceState();

            GameMaster.Instance.currentTurnState = new TurnState_Players();
            UIMaster.Instance.UpdateContinueButton(false);
            GameMaster.Instance.currentTurnState.StartState();
        }

        public override void UpdateState()
        {
            // If its our turn, update state
            if (GameMaster.Instance.currentPlayer == GameMaster.Instance.localPlayer)
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
            }
            else
                UIMaster.Instance.UpdateContinueButton(false);
            base.UpdateState();

        }

        // TODO: Add functionality to add cards to the amount based on extra variables (Memory Core cards)
        public void DrawDraftCards(int amount)
        {
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
                UIMaster.Instance.UpdateContinueButton(true);
                GameMaster.Instance.SetAllPlayersReady(false);
                draftStage = 2;
            }
            // If just finished picking cards, move to slotting
            else
            {
                UIMaster.Instance.draftPanel.EndDraft();
                GameMaster.Instance.SetAllPlayersReady(false);
                draftStage = 1;
            }
        }

        public virtual void CardSlotted()
        {
            GameMaster.Instance.currentPlayer.ready = true;
        }
    }
}