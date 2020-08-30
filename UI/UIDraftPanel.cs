using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class UIDraftPanel : UICardPanel
    {
        private bool useMiddleHeight = false;
        
        public void OpenDraftPanel(List<CommandCard> cardData)
        {            
            // NOTE: Temporary UI code for testing
            GetComponent<CanvasGroup>().alpha = 1f;
            useMiddleHeight = cardData.Count < 6;
            foreach (CommandCard card in cardData)
                AddCard(UIMaster.InstantiateCard(card, rect));
        }

        public void HideDraftPanel()
        {
            foreach (UICard element in cards)
                element.gameObject.SetActive(false);
            GetComponent<CanvasGroup>().alpha = 0f;
        }

        public void EndDraft()
        {
            ClearCards();
            GetComponent<CanvasGroup>().alpha = 0f;
        }

        public override void CardClicked(UICard card)
        {
            (GameMaster.Instance.currentTurnState as TurnState_Draft).CardPicked(card.cardData as CommandCard);
            UIMaster.Instance.handPanel.AddCard(card);
            cards.Remove(card);
        }

        protected override void PlaceCard(UICard card)
        {
            int i = cards.IndexOf(card);
            float posX = GetCardPosX(i);
            float posY = GetCardPosY(i);

            card.rect.position = rect.position + new Vector3(posX, posY, 0f);
            card.canvas.overrideSorting = false;
        }

        private float GetCardPosX(int cardIndex)
        {
            // TODO: Simplify
            float xPos;
            float panelWidth = GetComponent<RectTransform>().rect.size.x * GetComponentInParent<Canvas>().scaleFactor;
            float cardBuffer = panelWidth / 100f;
            float slotWidth = 0.2f * (panelWidth - 6 * cardBuffer);
            float step = slotWidth + cardBuffer;
            float startPos = (cardBuffer + slotWidth / 2) - panelWidth / 2;

            if (cardIndex < 5)
                xPos = startPos + cardIndex * step;
            else
                xPos = startPos + (cardIndex - 5) * step;

            return xPos;
        }

        private float GetCardPosY(int cardIndex)
        {
            // TODO: Simplify
            float yPos;
            float panelHeight = GetComponent<RectTransform>().rect.size.y * GetComponentInParent<Canvas>().scaleFactor;
            float heightBuffer = 3f * panelHeight / 100f;
            float slotHeight = 0.5f * (panelHeight - 3 * heightBuffer);
            float heightStep = slotHeight + heightBuffer;
            float heightStartPos = (heightBuffer + slotHeight / 2) - panelHeight / 2;

            if (useMiddleHeight)
                yPos = 0f;
            else
            {
                if (cardIndex < 5)
                    yPos = heightStartPos;
                else
                    yPos = -heightStartPos;
            }

            return yPos;
        }
    }
}
