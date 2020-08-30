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

        [Header("Damage effects")]
        public GameObject colorlessDamageEffect;
        public GameObject blueDamageEffect;
        public GameObject redDamageEffect;
        public GameObject yellowDamageEffect;
        public GameObject greenDamageEffect;

        [Header("UI")]
        public GameObject card;
        public GameObject commandCard;
        public GameObject draftCard;
        public GameObject handCard;

        [Header("Other")]
        public GameObject borderSquare;
    }
}