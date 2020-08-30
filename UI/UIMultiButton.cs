using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvM
{
    public class UIMultiButton : UIElement
    {
        public enum MultiButtonState
        {
            Inactive,
            Ready,
            Scrap,
            Tow
        }

        public MultiButtonState state = MultiButtonState.Inactive;

        private Button button;
        private Text text;

        protected override void Awake()
        {
            base.Awake();
            button = GetComponent<Button>();
            text = GetComponentInChildren<Text>();
        }

        public void ChangeState(MultiButtonState newState)
        { 
            state = newState;

            switch (state)
            {
                case MultiButtonState.Inactive:
                    button.interactable = false;
                    break;

                case MultiButtonState.Ready:
                    button.interactable = true;
                    text.text = "Ready";
                    break;

                case MultiButtonState.Scrap:
                    button.interactable = true;
                    text.text = "Scrap";
                    break;

                case MultiButtonState.Tow:
                    button.interactable = true;
                    break;

                default:
                    break;
            }
        }

        public void MultiButtonPressed()
        {
            switch (state)
            {
                case MultiButtonState.Ready:
                    GameMaster.Instance.currentPlayer.ready = true;
                    ChangeState(MultiButtonState.Inactive);
                    break;

                case MultiButtonState.Scrap:
                    GameMaster.Instance.Scrap();
                    break;

                case MultiButtonState.Tow:
                    break;

                default:
                    break;
            }
        }
    }
}