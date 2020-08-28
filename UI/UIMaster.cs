using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvM
{
    public class UIMaster : Singleton<UIMaster>
    {
        public enum UIState
        {
            Draft,
            Slot,
            ScrapRepair,
            ScrapSwap,
            PlayerSpawn,
            CmdLine,
            Wait
        }

        public UIState state;

        [Header("Main menu references")]
        public UIMainMenuPanel mainMenuPanel;

        [Header("During-game references")]
        public GameObject inGame;
        public GameObject turnState;
        public GameObject commandLine;
        public UIDraftPanel draftPanel;
        public UIHand handPanel;
        public UICardSlot[] cardSlots;
        public GameObject continueButton;
        public UIMultiButton multiButton;

        [Header("Panels")]
        public UIWinLosePanel winLostPanel;
        public UIScenarioInfoPanel scenarioInfoPanel;

        private Coroutine playerDamageAnimation;
        private UICardSlot selectedSwapItem;

        private void Start()
        {
            inGame.SetActive(false);
        }

        #region MainMenu and Initialization
        public void StartGame()
        {
            commandLine.SetActive(false);
            draftPanel.HideDraftPanel();
            mainMenuPanel.gameObject.SetActive(false);
        }

        public void ShowInGamePanel()
        {
            inGame.SetActive(true);
        }

        public void EndGame()
        {
            mainMenuPanel.gameObject.SetActive(true);
            inGame.SetActive(false);
            mainMenuPanel.scenarioSelection.value = 0;
            draftPanel.ClearDraftPanel();
            handPanel.ClearHand();
            handPanel.Hide();
            foreach (UICardSlot slot in cardSlots)
            {
                slot.SetHighlightState(UIHighlight.HighlightState.Inactive);
                slot.ClearCards();
            }
            winLostPanel.Hide();
        }
        #endregion

        #region InGame functionality
        public void UpdateMultiButtonState(UIMultiButton.MultiButtonState newState)
        {
            //Debug.Log("Updaring multibutton state to: " + newState);
            if (multiButton.state != newState)
                multiButton.ChangeState(newState);
        }

        public void ShowScenarioRules(Scenario scenario)
        {
            scenarioInfoPanel.ShowScenarioInfo(scenario.scenarioInfo);
        }

        public void ShowWinLostPanel(string winLoseInfo)
        {
            winLostPanel.ShowWinLosePanel(winLoseInfo);
        }

        public void ChangeTurnState(string state)
        {
            turnState.GetComponentInChildren<Text>().text = state;
        }

        // NOTE: Change so this is done for player specific commandlines, not only local.
        public void UpdateCommandLine()
        {
            foreach (UICardSlot slot in cardSlots)
                slot.UpdateSlotElements();
        }

        public bool PlayerDamageAnimationRunning()
        {
            if (playerDamageAnimation == null)
                return false;
            return true;
        }

        public void ToggleRepairScrap(Player player, bool scrapping)
        {
            handPanel.SetHandLocked(scrapping);
            if (scrapping)
            {
                for (int i = 0; i < player.commandLine.cards.Length; i++)
                {
                    if (player.commandLine.cards[i].Count > 0 && player.commandLine.cards[i].PeekTop() is DamageCard)
                        cardSlots[i].SetHighlightState(UIHighlight.HighlightState.Available);
                }
            }
            else
            {
                for (int i = 0; i < player.commandLine.cards.Length; i++)
                {
                        cardSlots[i].SetHighlightState(UIHighlight.HighlightState.Inactive);
                }
            }
        }

        public void ToggleSwapScrap(Player player, bool scrapping)
        {
            handPanel.SetHandLocked(scrapping);
            if (scrapping)
            {
                for (int i = 0; i < player.commandLine.cards.Length; i++)
                {
                    if (player.commandLine.cards[i].Count == 0 || !(player.commandLine.cards[i].PeekTop() is DamageCard))
                        cardSlots[i].SetHighlightState(UIHighlight.HighlightState.Available);
                }
            }
            else
            {
                for (int i = 0; i < player.commandLine.cards.Length; i++)
                {
                    cardSlots[i].SetHighlightState(UIHighlight.HighlightState.Inactive);
                }
            }
        }

        public UICardSlot SelectedSwapItem
        {
            get { return selectedSwapItem; }
            set { selectedSwapItem = value; }
        }

        public void SwapScrapInteraction(UICardSlot slot)
        {
            selectedSwapItem = slot;
            slot.SetHighlightState(UIHighlight.HighlightState.Selected);
        }
        #endregion
    }
}
