using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Skewer : BasicMoveCard
    {
        public bool hasMinionToken = false;

        public Skewer()
        {
            cardColor = Tools.Color.Blue;
            inputRequired = false;
            text = "Skewer";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_Skewer");
        }
    }
}