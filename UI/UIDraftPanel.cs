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

            for (int i = 0; i < turnStateScr.draftCards.Count; i++)
            {
                float xPos;
                if (i < 5)
                    xPos = -230f + i * 115f;
                else
                    xPos = -230f + (i - 5) * 115f;

                float yPos;
                if (turnStateScr.draftCards.Count <= 5)
                    yPos = 0f;
                else
                {
                    if (i < 5)
                        yPos = 70f;
                    else
                        yPos = -85f;
                }

                UIDraftCard card = Instantiate(Prefabs.Instance.draftCard, transform.position + new Vector3(xPos, yPos, 0f), Quaternion.identity, transform).GetComponent<UIDraftCard>();
                card.InitCard(turnStateScr.draftCards[i]);
                draftCards.Add(card);
            }
            hidden = false;
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
