using System;
using System.Collections.Generic;
using UnityEngine;

namespace CNFramework
{
    public class CNInput : MonoBehaviour
    {
        //TODO make this a getter/setter and manage that there is only one CNInput in the scene at all times, destroy all others
        private static CNInput _instance;
        
        [SerializeField] private Process inputProcess;
        
        //Button
        [SerializeField] private ButtonCondition pressCondition;
        private bool wasPressed;        
        
        //Axis1D
        [SerializeField] [Range(0,1)] private float axisTolerance = 0.75f;
        private bool wasActivated;
        
        private Dictionary<Handedness, Dictionary<ControllerInput, Delegate>> inputLookupTable;
        
        #region MonoBehaviour

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            
            
            //Initialize lookup tables
            inputLookupTable = new Dictionary<Handedness, Dictionary<ControllerInput, Delegate>>
            {
                { Handedness.Left, new Dictionary<ControllerInput, Delegate>() },
                { Handedness.Right, new Dictionary<ControllerInput, Delegate>() }
            };
        }

        private void FixedUpdate()
        {
            if (inputProcess == Process.FixedUpdate)
            {
                ProcessInput();
            }
        }

        private void Update()
        {
            if (inputProcess == Process.Update)
            {
                ProcessInput();
            }
        }

        private void LateUpdate()
        {
            if (inputProcess == Process.LateUpdate)
            {
                ProcessInput();
            }
        }

        private void OnPreRender()
        {
            if (inputProcess == Process.PreRender)
            {
                ProcessInput();
            }
        }

        private void OnPostRender()
        {
            if (inputProcess == Process.PostRender)
            {
                ProcessInput();
            }
        }

        private void OnPreCull()
        {
            if (inputProcess == Process.PreCull)
            {
                ProcessInput();
            }
        }

        #endregion MonoBehaviour

        #region Registration

        /// <summary>
        /// Registers an action to the CNInput system
        /// </summary>
        /// <param name="hand"><see cref="Handedness"/> used for the input</param>
        /// <param name="input"><see cref="ControllerInput"/> the system should be listening for</param>
        /// <param name="method">Action to call, passing a bool as the result</param>
        public static void Register(Handedness hand, ControllerInput input, Action<bool> method)
        {
            Register(hand, input, (Delegate)method);
        }
        
        /// <summary>
        /// Registers an action to the CNInput system
        /// </summary>
        /// <param name="hand"><see cref="Handedness"/> used for the input</param>
        /// <param name="input"><see cref="ControllerInput"/> the system should be listening for</param>
        /// <param name="method">Action to call, passing a float as the result</param>
        public static void Register(Handedness hand, ControllerInput input, Action<float> method)
        {
            Register(hand, input, (Delegate)method);
        }
        
        /// <summary>
        /// Registers an action to the CNInput system
        /// </summary>
        /// <param name="hand"><see cref="Handedness"/> used for the input</param>
        /// <param name="input"><see cref="ControllerInput"/> the system should be listening for</param>
        /// <param name="method">Action to call, passing a Vector2 as the result</param>
        public static void Register(Handedness hand, ControllerInput input, Action<Vector2> method)
        {
            Register(hand, input, (Delegate)method);
        }

        private static void Register(Handedness hand, ControllerInput input, Delegate delegation)
        {
            if (!_instance.inputLookupTable.TryGetValue(hand, out Dictionary<ControllerInput, Delegate> lookup))
            {
                lookup = new Dictionary<ControllerInput, Delegate>();
                _instance.inputLookupTable.Add(hand, lookup);
            }

            if (lookup.TryGetValue(input, out Delegate del))
            {
                lookup[input] = Delegate.Combine(del, delegation);
            }
            else
            {
                lookup.Add(input, delegation);
            }
        }

        #endregion Registration
        
        #region Unregistration
        
        /// <summary>
        /// Unregisters an action previously registered to the CNInput system
        /// </summary>
        /// <param name="hand"><see cref="Handedness"/> used for the input</param>
        /// <param name="input"><see cref="ControllerInput"/> the system was listening for</param>
        /// <param name="method">Action that was called, passing a bool as the result</param>
        public static void Unregister(Handedness hand, ControllerInput input, Action<bool> method)
        {
            Unregister(hand, input, (Delegate)method);
        }
        
        /// <summary>
        /// Unregisters an action previously registered to the CNInput system
        /// </summary>
        /// <param name="hand"><see cref="Handedness"/> used for the input</param>
        /// <param name="input"><see cref="ControllerInput"/> the system was listening for</param>
        /// <param name="method">Action that was called, passing a float as the result</param>
        public static void Unregister(Handedness hand, ControllerInput input, Action<float> method)
        {
            Unregister(hand, input, (Delegate)method);
        }
        
        /// <summary>
        /// Unregisters an action previously registered to the CNInput system
        /// </summary>
        /// <param name="hand"><see cref="Handedness"/> used for the input</param>
        /// <param name="input"><see cref="ControllerInput"/> the system was listening for</param>
        /// <param name="method">Action that was called, passing a Vector2 as the result</param>
        public static void Unregister(Handedness hand, ControllerInput input, Action<Vector2> method)
        {
            Unregister(hand, input, (Delegate)method);
        }

        private static void Unregister(Handedness hand, ControllerInput input, Delegate delegation)
        {
            if (!_instance.inputLookupTable.TryGetValue(hand, out Dictionary<ControllerInput, Delegate> lookup) ||
                !lookup.TryGetValue(input, out Delegate del))
            {
                return;
            }
            
            lookup[input] = Delegate.Remove(del, delegation);
        }
        
        #endregion Unregistration

        #region Processing
        
        private void ProcessInput()
        {
            var leftHandLookup = inputLookupTable[Handedness.Left];
            
            //Left hand button processes
            if (leftHandLookup.ContainsKey(ControllerInput.InnerFace)) ProcessButton(Handedness.Left, ControllerInput.InnerFace);
            if (leftHandLookup.ContainsKey(ControllerInput.OuterFace)) ProcessButton(Handedness.Left, ControllerInput.OuterFace);
            if (leftHandLookup.ContainsKey(ControllerInput.ThumbStickPress)) ProcessButton(Handedness.Left, ControllerInput.ThumbStickPress);
            if (leftHandLookup.ContainsKey(ControllerInput.ThumbStickTouch)) ProcessButton(Handedness.Left, ControllerInput.ThumbStickTouch);
            if (leftHandLookup.ContainsKey(ControllerInput.TriggerTouch)) ProcessButton(Handedness.Left, ControllerInput.TriggerTouch);
            
            //Left hand axis1D processes
            if (leftHandLookup.ContainsKey(ControllerInput.GripAxis)) ProcessAxis1D(Handedness.Left, ControllerInput.GripAxis);
            if (leftHandLookup.ContainsKey(ControllerInput.TriggerAxis)) ProcessAxis1D(Handedness.Left, ControllerInput.TriggerAxis);
            if (leftHandLookup.ContainsKey(ControllerInput.IndexFingerCapacitance)) ProcessAxis1D(Handedness.Left, ControllerInput.IndexFingerCapacitance);
            if (leftHandLookup.ContainsKey(ControllerInput.MiddleFingerCapacitance)) ProcessAxis1D(Handedness.Left, ControllerInput.MiddleFingerCapacitance);
            if (leftHandLookup.ContainsKey(ControllerInput.RingFingerCapacitance)) ProcessAxis1D(Handedness.Left, ControllerInput.RingFingerCapacitance);
            if (leftHandLookup.ContainsKey(ControllerInput.PinkyFingerCapacitance)) ProcessAxis1D(Handedness.Left, ControllerInput.PinkyFingerCapacitance);
            
            //Left hand axis2D processes
            if (leftHandLookup.ContainsKey(ControllerInput.ThumbStickAxis)) ProcessAxis2D(Handedness.Left);

            var rightHandLookup = inputLookupTable[Handedness.Right];
            
            //Right hand process
            if (rightHandLookup.ContainsKey(ControllerInput.InnerFace)) ProcessButton(Handedness.Right, ControllerInput.InnerFace);
            if (rightHandLookup.ContainsKey(ControllerInput.OuterFace)) ProcessButton(Handedness.Right, ControllerInput.OuterFace);
            if (rightHandLookup.ContainsKey(ControllerInput.ThumbStickPress)) ProcessButton(Handedness.Right, ControllerInput.ThumbStickPress);
            if (rightHandLookup.ContainsKey(ControllerInput.ThumbStickTouch)) ProcessButton(Handedness.Right, ControllerInput.ThumbStickTouch);
            if (rightHandLookup.ContainsKey(ControllerInput.TriggerTouch)) ProcessButton(Handedness.Right, ControllerInput.TriggerTouch);
            
            //Right hand axis1D processes
            if (rightHandLookup.ContainsKey(ControllerInput.GripAxis)) ProcessAxis1D(Handedness.Right, ControllerInput.GripAxis);
            if (rightHandLookup.ContainsKey(ControllerInput.TriggerAxis)) ProcessAxis1D(Handedness.Right, ControllerInput.TriggerAxis);
            if (rightHandLookup.ContainsKey(ControllerInput.IndexFingerCapacitance)) ProcessAxis1D(Handedness.Right, ControllerInput.IndexFingerCapacitance);
            if (rightHandLookup.ContainsKey(ControllerInput.MiddleFingerCapacitance)) ProcessAxis1D(Handedness.Right, ControllerInput.MiddleFingerCapacitance);
            if (rightHandLookup.ContainsKey(ControllerInput.RingFingerCapacitance)) ProcessAxis1D(Handedness.Right, ControllerInput.RingFingerCapacitance);
            if (rightHandLookup.ContainsKey(ControllerInput.PinkyFingerCapacitance)) ProcessAxis1D(Handedness.Right, ControllerInput.PinkyFingerCapacitance);
            
            //Right hand axis2D processes
            if (rightHandLookup.ContainsKey(ControllerInput.ThumbStickAxis)) ProcessAxis2D(Handedness.Right);
        }

        private void ProcessButton(Handedness hand, ControllerInput input)
        {
            var keyCode = KeyCode.A;
            
            switch (input)
            {
                case ControllerInput.InnerFace:
                    keyCode = hand == Handedness.Left ? KeyCode.JoystickButton2 : KeyCode.JoystickButton0;
                    break;
                case ControllerInput.OuterFace:
                    keyCode = hand == Handedness.Left ? KeyCode.JoystickButton3 : KeyCode.JoystickButton1;
                    break;
                case ControllerInput.ThumbStickPress:
                    keyCode = hand == Handedness.Left ? KeyCode.JoystickButton8 : KeyCode.JoystickButton9;
                    break;
                case ControllerInput.ThumbStickTouch:
                    keyCode = hand == Handedness.Left ? KeyCode.JoystickButton16 : KeyCode.JoystickButton17;
                    break;
                case ControllerInput.TriggerTouch:
                    keyCode = hand == Handedness.Left ? KeyCode.JoystickButton14 : KeyCode.JoystickButton15;
                    break;
            }

            switch (pressCondition)
            {
                case ButtonCondition.Constant:
                    var result = Input.GetKey(keyCode);
                    var constantAction = RetrieveDelegation(hand, input) as Action<bool>;
                    if (constantAction == null) return;
                    constantAction(result);
                    break;
                case ButtonCondition.DownPress:
                    if (Input.GetKeyDown(keyCode))
                    {
                        var downAction = RetrieveDelegation(hand, input) as Action<bool>;
                        if (downAction == null) return;
                        downAction(true);
                    }
                    break;
                case ButtonCondition.UpPress:
                    if (Input.GetKeyUp(keyCode))
                    {
                        var upAction = RetrieveDelegation(hand, input) as Action<bool>;
                        if (upAction == null) return;
                        upAction(true); 
                    }
                    break;
            }
        }

        private void ProcessAxis1D(Handedness hand, ControllerInput input)
        {
            var axisName = string.Empty;

            switch (input)
            {
                case ControllerInput.GripAxis:
                    axisName = hand == Handedness.Left ? "LeftGrip" : "RightGrip";
                    break;
                case ControllerInput.TriggerAxis:
                    axisName = hand == Handedness.Left ? "LeftTrigger" : "RightTrigger";
                    break;
                case ControllerInput.IndexFingerCapacitance: 
                    axisName = hand == Handedness.Left ? "LeftIndexCapacitance" : "RightIndexCapacitance";
                    break;
                case ControllerInput.MiddleFingerCapacitance: 
                    axisName = hand == Handedness.Left ? "LeftMiddleCapacitance" : "RightMiddleCapacitance";
                    break;
                case ControllerInput.RingFingerCapacitance: 
                    axisName = hand == Handedness.Left ? "LeftRingCapacitance" : "RightRingCapacitance";
                    break;
                case ControllerInput.PinkyFingerCapacitance:
                    axisName = hand == Handedness.Left ? "LeftPinkyCapacitance" : "RightPinkyCapacitance";
                    break;
            }

            var result = Input.GetAxis(axisName);
            if (result >= axisTolerance)
            {
                var action = RetrieveDelegation(hand, input) as Action<float>;
                if (action == null) return;
                action(result);
            }
        }

        private void ProcessAxis2D(Handedness hand)
        {
            var xAxisName = hand == Handedness.Left ? "LeftHorizontal" : "RightHorizontal";
            var yAxisName = hand == Handedness.Left ? "LeftVertical" : "RightVertical";

            var action = RetrieveDelegation(hand, ControllerInput.ThumbStickAxis) as Action<Vector2>;
            if (action == null) return;
            
            var xResult = Input.GetAxis(xAxisName);
            var yResult = Input.GetAxis(yAxisName);

            action(new Vector2(xResult, yResult));
        }
        
        #endregion Processing

        private Delegate RetrieveDelegation(Handedness hand, ControllerInput input)
        {
            if (_instance.inputLookupTable.TryGetValue(hand, out Dictionary<ControllerInput, Delegate> lookup) &&
                lookup.TryGetValue(input, out Delegate del))
            {
                return del;
            }

            return null;
        }
    }

    #region Enumeration
    
    public enum ControllerInput
    {
        /// <summary>Input value most commonly used for the menu button.
        /// Mapped to <see cref="KeyCode.JoystickButton0"/>(Right) and <see cref="KeyCode.JoystickButton2"/>(Left) </summary>
        InnerFace,
        /// <summary>Input value used on Valve Knuckle controllers for a second face button.
        /// Mapped to <see cref="KeyCode.JoystickButton1"/>(Right) and <see cref="KeyCode.JoystickButton3"/>(Left) </summary>
        OuterFace,
        /// <summary>Input value used when the thumb stick is press down(touch pad on Vive controllers).
        /// Mapped to <see cref="KeyCode.JoystickButton9"/>(Right) and <see cref="KeyCode.JoystickButton8"/>(Left)</summary>
        ThumbStickPress,
        /// <summary>Input value used when the thumb stick is touched(touch pad on Vive controllers).
        /// Mapped to <see cref="KeyCode.JoystickButton17"/>(Right) and <see cref="KeyCode.JoystickButton16"/>(Left)</summary>
        ThumbStickTouch,
        /// <summary>Input value used when the thumb stick is moved(touch pad surface on Vive controllers).
        /// Mapped to X axis(LeftX), Y axis(LeftY) and 4th axis(RightX), 5th axis(RightY)</summary>
        ThumbStickAxis,
        /// <summary>Input value used when the trigger is touched.
        /// Mapped to <see cref="KeyCode.JoystickButton15"/>(Right) and <see cref="KeyCode.JoystickButton14"/>(Left)</summary>
        TriggerTouch,
        /// <summary>Input value used when the trigger is squeezed.
        /// Mapped to 9th axis(Left) and 10th axis(Right)</summary>
        TriggerAxis,
        /// <summary>Input value used when the grip is squeezed.
        /// Mapped to 11th axis(Left) and 12th axis(Right)</summary>bu
        GripAxis,
        /// <summary>Input value used when the index finger is gripping the controller(Valve knuckles only).
        /// Mapped to 20th axis(Left) and 21st axis(Right)</summary>
        IndexFingerCapacitance,
        /// <summary>Input value used when the middle finger is gripping the controller(Valve knuckles only).
        /// Mapped to 22nd axis(Left) and 23rd axis(Right)</summary>
        MiddleFingerCapacitance,
        /// <summary>Input value used when the ring finger is gripping the controller(Valve knuckles only).
        /// Mapped to 24th axis(Left) and 25th axis(Right)</summary>
        RingFingerCapacitance,
        /// <summary>Input value used when the pinky finger is gripping the controller(Valve knuckles only).
        /// Mapped to 26th axis(Left) and 27th axis(Right)</summary>
        PinkyFingerCapacitance
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
    
    public enum Handedness
    {
        /// <summary>Selection representing the left hand</summary>
        Left,
        /// <summary>Selection representing the right hand</summary>
        Right
    }
    
    #endregion Enumeration

}
