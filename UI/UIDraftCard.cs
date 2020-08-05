using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvM
{
    public class UIDraftCard : UIElement
    {        
        [Header("UI Object references")]
        public Text text;
        public Image BG;
        private UIDraftPanel choicePanel;

        [Header("Data")]
        public Card draftCard;

        private void OnEnable()
        {
            choicePanel = GetComponentInParent<UIDraftPanel>();
        }

        public void InitCard(Card card)
        {
            draftCard = card;
            if (card.textureAsset != null)
                BG.sprite = card.textureAsset;

            if (card is DamageCard || card.textureAsset == null)
                text.text = draftCard.text;

            if (card is CommandCard)
                text.color = Tools.GetColor((card as CommandCard).cardColor);
            else if (card is DamageCard)
                text.color = Color.black;
        }

        public virtual void SelectChoice()
        {
            GameMaster.Instance.currentPlayer.hand.Add(draftCard); // NOTE: This should be moved elsewhere. Data handling should not happen in UI code.
            UIMaster.Instance.handPanel.AddCard(draftCard);

            choicePanel.SelectCard(this);
        }
        
    }
}
