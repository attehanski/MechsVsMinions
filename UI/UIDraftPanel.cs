using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class UIDraftPanel : UIElement
    {
        public List<UIDraftCard> draftCards = new List<UIDraftCard>();
        public bool hidden = true;

        // NOTE: Should split this into multiple parts
        public void OpenDraftPanel()
        {
            if (!(GameMaster.Instance.currentTurnState is TurnState_Draft))
            {
                Debug.LogError("Tried opening draft panel while turn is not in draft state!");
                return;
            }
            
            GetComponent<CanvasGroup>().alpha = 1f;
            TurnState_Draft turnStateScr = (TurnState_Draft)GameMaster.Instance.currentTurnState;
            float panelWidth = GetComponent<RectTransform>().rect.size.x * GetComponentInParent<Canvas>().scaleFactor;
            float cardBuffer = panelWidth / 100f;
            float slotWidth = 0.2f * (panelWidth - 6 * cardBuffer);
            float step = slotWidth + cardBuffer;
            float startPos = (cardBuffer + slotWidth / 2) - panelWidth / 2;

            float panelHeight = GetComponent<RectTransform>().rect.size.y * GetComponentInParent<Canvas>().scaleFactor;
            float heightBuffer = 3f * panelHeight / 100f;
            float slotHeight = 0.5f * (panelHeight - 3 * heightBuffer);
            float heightStep = slotHeight + heightBuffer;
            float heightStartPos = (heightBuffer + slotHeight / 2) - panelHeight / 2;

            for (int i = 0; i < turnStateScr.draftCards.Count; i++)
            {
                float xPos;
                if (i < 5)
                    xPos = startPos + i * step;
                else
                    xPos = startPos + (i - 5) * step;

                float yPos;
                if (turnStateScr.draftCards.Count <= 5)
                    yPos = 0f;
                else
                {
                    if (i < 5)
                        yPos = heightStartPos;
                    else
                        yPos = -heightStartPos;
                }

                UIDraftCard card = Instantiate(Prefabs.Instance.draftCard, transform).GetComponent<UIDraftCard>();
                card.transform.position = transform.position + new Vector3(xPos, yPos, 0f);
                card.transform.rotation = Quaternion.identity;
                card.GetComponent<Canvas>().overrideSorting = false;
                card.InitCard(turnStateScr.draftCards[i]);
                draftCards.Add(card);
            }
            hidden = false;
        }

        public void ClearDraftPanel()
        {
            int lastIndex = draftCards.Count - 1;
            for (int i = 0; i < draftCards.Count; i++)
            {
                Destroy(draftCards[lastIndex - i].gameObject);
            }
            draftCards.Clear();
        }

        public void HideDraftPanel()
        {
            foreach (UIDraftCard element in draftCards)
                element.gameObject.SetActive(false);
            GetComponent<CanvasGroup>().alpha = 0f;
            hidden = true;
        }

        public void SelectCard(UIDraftCard draftCard)
        {
            draftCards.Remove(draftCard);
            Destroy(draftCard.gameObject);

            GameMaster.Instance.currentPlayer.ready = true;
        }

        public void EndDraft()
        {
            foreach (UIDraftCard card in draftCards)
            {
                GameMaster.Instance.DiscardCard(card.draftCard); // NOTE: This should be moved elsewhere. Data handling should not happen in UI code.
                Destroy(card.gameObject);
            }
            draftCards.Clear();
            GetComponent<CanvasGroup>().alpha = 0f;
            hidden = true;
        }

        public void CardSlotted()
        {
            TurnState_Draft turnStateScr = (TurnState_Draft)GameMaster.Instance.currentTurnState;
            turnStateScr.CardSlotted();
        }
    }
}
