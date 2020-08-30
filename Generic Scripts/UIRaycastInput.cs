using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MvM
{
    public class UIRaycastInput : MonoBehaviour
    {
        private GraphicRaycaster raycaster;
        private Ray ray;
        private RaycastHit hit;

        void Awake()
        {
            raycaster = GetComponent<GraphicRaycaster>();
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {

            }

            if (Input.GetKeyUp(KeyCode.Mouse2))
            {

            }
        }
    }
}