using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvM
{
    public class UIMainMenuPanel : MonoBehaviour
    {
        public Button playButton;
        public Dropdown scenarioSelection;

        private void Start()
        {
            playButton.interactable = false;
        }

        public void ChangeScenario(int scenarioValue)
        {
            switch((Scenarios.AllScenarios)scenarioValue)
            {
                case Scenarios.AllScenarios.None:
                    playButton.interactable = false;
                    GameMaster.Instance.SetScenario(null);
                    break;

                case Scenarios.AllScenarios.TestScenario:
                    playButton.interactable = true;
                    GameMaster.Instance.SetScenario(new TestScenario());
                    break;

                case Scenarios.AllScenarios.Scenario0:
                    playButton.interactable = true;
                    GameMaster.Instance.SetScenario(new Scenario0());
                    break;

                default:
                    break;
            }
        }
    }
}