using System;
using UnityEngine;

namespace CNFramework
{
    public class ButtonInputAction : InputAction
    {
        [SerializeField] private KeyCode inputKey = KeyCode.JoystickButton0;
        [SerializeField] private ButtonCondition condition;
        [SerializeField] private bool result;

        public event Action OnButtonDown;
        public event Action OnButtonUp;
        public event Action<bool> OnButton;

        private bool lastResult;

        protected override void ProcessAction()
        {
            result = Input.GetKey(inputKey);

            switch (condition)
            {
                case ButtonCondition.DownPress:
                    if (!lastResult && result)
                    {
                        OnButtonDown?.Invoke();
                    }
                    break;
                
                case ButtonCondition.UpPress:
                    if (lastResult && !result)
                    {
                        OnButtonUp?.Invoke();
                    }
                    break;
                
                case ButtonCondition.Constant:
                    OnButton?.Invoke(result);
                    break;
            }

            lastResult = result;
        }
    }

    
}
