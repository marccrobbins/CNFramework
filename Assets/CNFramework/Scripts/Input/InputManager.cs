using System;
using UnityEngine;

namespace CNFramework
{
    public class InputManager : MonoBehaviour
    {
        public Process InputProcess;
        public float axisThreshold;
        public ButtonCondition buttonCondition;
        
        private void Start()
        {
            //Collect all input type and apply settings accordingly
            var collectedAxis2DActions = GetComponentsInChildren<InputAction<Vector2>>();
            var collectedAxis1DActions = GetComponentsInChildren<InputAction<float>>();
            var collectedButtonActions = GetComponentsInChildren<InputAction<bool>>();

            foreach (var axis2DAction in collectedAxis2DActions)
            {
                axis2DAction.ApplySettings();
            }
        }
    }
    
    public enum Process
    {
        /// <summary>Input is processed in the Update unity event</summary>
        Update = 0,
        /// <summary>Input is processed in the LateUpdate unity event</summary>
        LateUpdate,
        /// <summary>Input is processed in the FixedUpdate unity event</summary>
        FixedUpdate,
        /// <summary>Input is processed in the OnPreRender unity event</summary>
        PreRender,
        /// <summary>Input is processed in the OnPostRender unity event</summary>
        PostRender,
        /// <summary>Input is processed in the OnPreCull unity event</summary>
        PreCull
    }
    
    public enum ButtonCondition
    {
        /// <summary>Button inputs only listen for up presses</summary>
        UpPress = 0,
        /// <summary>Button inputs only listen for down presses</summary>
        DownPress,
        /// <summary>Button inputs listen for any press</summary>
        Constant
    }
}
