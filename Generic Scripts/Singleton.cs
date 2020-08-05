using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance)
            {
                Debug.LogError("Singleton " + gameObject + " already has an instance on awake!");
                Destroy(gameObject);
            }
            else
                Instance = (T)FindObjectOfType(typeof(T));
        }
    }
}
