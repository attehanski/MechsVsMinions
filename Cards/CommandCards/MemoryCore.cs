using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class MemoryCore : BasicTurnCard
    {
        public MemoryCore()
        {
            cardColor = Tools.Color.Green;
            inputRequired = true;
            text = "Memory Core";
            textureAsset = Resources.Load<Sprite>("CommandCardTextures/T_MemoryCore");
        }
    }
}