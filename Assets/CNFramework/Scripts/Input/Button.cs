using UnityEngine;

namespace CNFramework
{
    public class Button : InputAction<bool>
    {
        [FilteredKeycode("JoystickButton")] 
        public KeyCode buttonCode;
        public ButtonActivation buttonActivation;

       private void Start()
        {
            if (buttonCode == KeyCode.None)
            {
                Debug.LogError("There is no button key code set.");
                return;
            }

            IsValid = true;
        }

        private void Update()
        {
            if (!IsValid) return;

            switch (buttonActivation)
            {
                case ButtonActivation.OnUp:
                    value = Input.GetKeyUp(buttonCode);
                    break;
                case ButtonActivation.OnDown:
                    value = Input.GetKeyDown(buttonCode);
                    break;
                default:
                    value = Input.GetKey(buttonCode);
                    break;
            }
            
            ProcessValue(value);
        }
    }
}
