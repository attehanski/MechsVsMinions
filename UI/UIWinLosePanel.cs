using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvM
{
    public class UIWinLosePanel : UIElement
    {
        public Text text;

        public void SetWinLoseText(string winLoseText)
        {
            text.text = winLoseText;
        }

        public void ShowWinLosePanel(string info)
        {
            SetWinLoseText(info);
            gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}