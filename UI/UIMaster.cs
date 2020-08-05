using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvM
{
    public class UIMaster : Singleton<UIMaster>
    {
        [Header("Main menu references")]
        public GameObject mainMenuPanel;

        [Header("During-game references")]
        public GameObject inGame;
        public GameObject turnState;
        public GameObject commandLine;
        public UIDraftPanel draftPanel;
        public UIHand handPanel;
        public UICardSlot[] cardSlots;
        public GameObject continueButton;

        private Coroutine playerDamageAnimation;

        private void Start()
        {
            inGame.SetActive(false);
        }

        #region MainMenu and Initialization
        public void StartGame()
        {
            commandLine.SetActive(false);
            draftPanel.HideDraftPanel();
            mainMenuPanel.SetActive(false);
        }

        public void ShowInGamePanel()
        {
            inGame.SetActive(true);
        }
        #endregion

        #region InGame functionality
        public void UpdateContinueButton(bool enable)
        {
            if (continueButton.activeInHierarchy && !enable)
            {
                continueButton.SetActive(false);
                //Debug.Log("UpdateContinueButton " + enable);
            }
            else if (!continueButton.activeInHierarchy && enable)
            {
                EnableContinueButton();
                //Debug.Log("UpdateContinueButton " + enable);
            }
        }

        public void EnableContinueButton(bool endRound = false)
        {
            continueButton.SetActive(true);
            if (endRound)
                continueButton.GetComponentInChildren<Text>().text = "End round";
            else
                continueButton.GetComponentInChildren<Text>().text = "Ready";
        }

        public void ShowScenarioRules(Scenario scenario)
        {
            //EnableContinueButton();
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
        #endregion
    }
}
