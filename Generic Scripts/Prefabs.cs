using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Prefabs : Singleton<Prefabs>
    {
        [Header("Units")]
        public GameObject minion;
        public GameObject genericChampion;

        [Header("UI")]
        public GameObject commandCard;
        public GameObject draftCard;
        public GameObject handCard; // NOTE: Not sure if this is the best way to differentiate different UI cards

        [Header("Other")]
        public GameObject borderSquare;
    }
}