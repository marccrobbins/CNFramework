using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNFramework
{
    public class Button : MonoBehaviour
    {
        public KeyCode buttonCode;
        public bool isPressed;

        public bool IsValid { get; private set; }
        
        void Start()
        {
            if (buttonCode == KeyCode.None)
            {
                Debug.LogError("There is no button key code set.");
                return;
            }

            IsValid = true;
        }

        void Update()
        {
            if (!IsValid) return;

            isPressed = Input.GetKey(buttonCode);
        }
    }
}
