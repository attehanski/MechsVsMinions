using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvM
{
    public class UICardSlot : UIElement
    {
        public int index;
        private List<GameObject> slotCards = new List<GameObject>();

        public void OnCardSlotClicked()
        {
            GameMaster.Instance.CardSlotInteracted(this);
        }

        public void SlotCard(Card card)
        {
            UpdateSlotElements();
        }

        public void GenerateUICard(Card card, int index)
        {
            GameObject tempCard = Instantiate(Prefabs.Instance.commandCard, transform.position + new Vector3(0f, 15 - 15 * index, 0f), Quaternion.identity, transform);

            if (card.textureAsset != null)
                tempCard.GetComponent<Image>().sprite = card.textureAsset;

            if (card is DamageCard || card.textureAsset == null)
                tempCard.GetComponentInChildren<Text>().text = card.text;

            if (card is CommandCard)
                tempCard.GetComponentInChildren<Text>().color = Tools.GetColor((card as CommandCard).cardColor);
            else if (card is DamageCard)
                tempCard.GetComponentInChildren<Text>().color = Color.black;
            slotCards.Add(tempCard);
        }

        public void UpdateSlotElements()
        {
            // Destroy old cards, if any
            foreach (GameObject card in slotCards)
                Destroy(card);
            slotCards.Clear();
            int i = 0;

            // Generate new cards
            foreach (Card card in GameMaster.Instance.localPlayer.commandLine.cards[index].Reversed)
            {
                GenerateUICard(card, i);
                i++;
            }
        }
    }
}
