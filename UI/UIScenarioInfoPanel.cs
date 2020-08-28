using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvM
{
    public class UIScenarioInfoPanel : UIElement
    {
        public Text text;

        public void SetScenarioInfo(string infoText)
        {
            text.text = infoText;
        }

        public void ShowScenarioInfo(string info)
        {
            SetScenarioInfo(info);
            gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}