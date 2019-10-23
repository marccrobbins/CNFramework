using System;
using UnityEngine;

namespace CNFramework
{
    public class ControllerInputManager : MonoBehaviour
    {
        [SerializeField] private ButtonInputAction menuButton;
        public ButtonInputAction MenuButton => menuButton;
        
        [Header("Joystick")]
        [SerializeField] private Axis2DAction joystickAxis;
        public Axis2DAction JoystickAxis => joystickAxis;
        [SerializeField] private ButtonInputAction joystickTouchButton;
        public ButtonInputAction JoystickTouchButton => joystickTouchButton;
        [SerializeField] private ButtonInputAction joystickClickButton;
        public ButtonInputAction JoystickClickButton => joystickClickButton;
        
        [Header("Trigger")]
        [SerializeField] private Axis1DAction triggerAxis;
        public Axis1DAction TriggerAxis => triggerAxis;
        [SerializeField] private ButtonInputAction triggerTouchButton;
        public ButtonInputAction TriggerTouchButton => triggerTouchButton;
        
        [Header("Grip")]
        [SerializeField] private Axis1DAction gripAxis;
        public Axis1DAction GripAxis => gripAxis;
    }
}


