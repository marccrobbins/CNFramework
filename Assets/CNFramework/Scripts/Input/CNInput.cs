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
        
        private Dictionary<Handedness, Dictionary<ControllerInput, Delegation>> inputLookupDelegation;
        
        #region MonoBehaviour

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            
            
            //Initialize lookup table
            inputLookupDelegation = new Dictionary<Handedness, Dictionary<ControllerInput, Delegation>>
            {
                { Handedness.Left, new Dictionary<ControllerInput, Delegation>() },
                { Handedness.Right, new Dictionary<ControllerInput, Delegation>() }
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
        /// <param name="activatedMethod">Action to call when input is activated, passing a <see cref="bool"/> as the result</param>
        /// /// <param name="deactivatedMethod">Action to call when input is deactivated, passing a <see cref="bool"/> as the result</param>
        /// /// <param name="changedMethod">Action to call when input is changed, passing a <see cref="bool"/> as the result</param>
        public static void Register(Handedness hand, ControllerInput input, Action<bool, Handedness> activatedMethod = null, Action<bool, Handedness> deactivatedMethod = null, Action<bool, Handedness> changedMethod = null)
        {
            RegisterDelegation(hand, input, activatedMethod, deactivatedMethod, changedMethod);
        }
        
        /// <summary>
        /// Registers an action to the CNInput system
        /// </summary>
        /// <param name="hand"><see cref="Handedness"/> used for the input</param>
        /// <param name="input"><see cref="ControllerInput"/> the system should be listening for</param>
        /// <param name="activatedMethod">Action to call when input is activated, passing a <see cref="float"/> as the result</param>
        /// /// <param name="deactivatedMethod">Action to call when input is deactivated, passing a <see cref="float"/> as the result</param>
        /// /// <param name="changedMethod">Action to call when input is changed, passing a <see cref="float"/> as the result</param>
        public static void Register(Handedness hand, ControllerInput input, Action<float, Handedness> activatedMethod = null, Action<float, Handedness> deactivatedMethod = null, Action<float, Handedness> changedMethod = null)
        {
            RegisterDelegation(hand, input, activatedMethod, deactivatedMethod, changedMethod);
        }
        
        /// <summary>
        /// Registers an action to the CNInput system
        /// </summary>
        /// <param name="hand"><see cref="Handedness"/> used for the input</param>
        /// <param name="input"><see cref="ControllerInput"/> the system should be listening for</param>
        /// <param name="activatedMethod">Action to call when input is activated, passing a <see cref="Vector2"/> as the result</param>
        public static void Register(Handedness hand, ControllerInput input, Action<Vector2, Handedness> activatedMethod = null)
        {
            RegisterDelegation(hand, input, activatedMethod);
        }

        private static void RegisterDelegation(Handedness hand, ControllerInput input, Delegate activatedDel = null, Delegate deactivatedDel = null, Delegate changedDel = null)
        {
            //Validate that there is something to register
            if (activatedDel == null &&
                deactivatedDel == null &&
                changedDel == null)
            {
                Debug.LogErrorFormat("There were no actions to register for {0} hand with {1} input", hand, input);
                return;
            }
            
            if (!_instance.inputLookupDelegation.TryGetValue(hand, out Dictionary<ControllerInput, Delegation> lookup))
            {
                lookup = new Dictionary<ControllerInput, Delegation>();
                _instance.inputLookupDelegation.Add(hand, lookup);
            }

            if (lookup.TryGetValue(input, out Delegation delegation))
            {
                if (activatedDel != null)
                {
                    delegation.OnActivated = Delegate.Combine(delegation.OnActivated, activatedDel);
                }
                
                if (deactivatedDel != null)
                {
                    delegation.OnDeactivated = Delegate.Combine(delegation.OnDeactivated, deactivatedDel);
                }

                if (changedDel != null)
                {
                    delegation.OnChanged = Delegate.Combine(delegation.OnChanged, changedDel);
                }
                
                lookup[input] = delegation;
            }
            else
            {
                lookup.Add(input, new Delegation
                {
                    OnActivated = activatedDel,
                    OnDeactivated = deactivatedDel,
                    OnChanged = changedDel
                });
            }
        }

        #endregion Registration
        
        #region Unregistration
        
        /// <summary>
        /// Unregisters an action previously registered to the CNInput system
        /// </summary>
        /// <param name="hand"><see cref="Handedness"/> used for the input</param>
        /// <param name="input"><see cref="ControllerInput"/> the system was listening for</param>
        /// <param name="activatedMethod">Action that was called when input is activated, passing a <see cref="bool"/> as the result</param>
        /// /// <param name="deactivatedMethod">Action that was called when input is deactivated, passing a <see cref="bool"/> as the result</param>
        /// /// <param name="changedMethod">Action that was called when input is changed, passing a <see cref="bool"/> as the result</param>
        public static void Unregister(Handedness hand, ControllerInput input, Action<bool, Handedness> activatedMethod = null, Action<bool, Handedness> deactivatedMethod = null, Action<bool, Handedness> changedMethod = null)
        {
            UnregisterDelegation(hand, input, activatedMethod, deactivatedMethod, changedMethod);
        }
        
        /// <summary>
        /// Unregisters an action previously registered to the CNInput system
        /// </summary>
        /// <param name="hand"><see cref="Handedness"/> used for the input</param>
        /// <param name="input"><see cref="ControllerInput"/> the system was listening for</param>
        /// <param name="activatedMethod">Action that was called when input is activated, passing a <see cref="float"/> as the result</param>
        /// /// <param name="deactivatedMethod">Action that was called when input is deactivated, passing a <see cref="float"/> as the result</param>
        /// /// <param name="changedMethod">Action that was called when input is changed, passing a <see cref="float"/> as the result</param>
        public static void Unregister(Handedness hand, ControllerInput input, Action<float, Handedness> activatedMethod = null, Action<float, Handedness> deactivatedMethod = null, Action<float, Handedness> changedMethod = null)
        {
            UnregisterDelegation(hand, input, activatedMethod, deactivatedMethod, changedMethod);
        }
        
        /// <summary>
        /// Unregisters an action previously registered to the CNInput system
        /// </summary>
        /// <param name="hand"><see cref="Handedness"/> used for the input</param>
        /// <param name="input"><see cref="ControllerInput"/> the system was listening for</param>
        /// <param name="activatedMethod">Action that was called when input is activated, passing a <see cref="Vector2"/> as the result</param>
        public static void Unregister(Handedness hand, ControllerInput input, Action<Vector2, Handedness> activatedMethod)
        {
            UnregisterDelegation(hand, input, activatedMethod);
        }

        private static void UnregisterDelegation(Handedness hand, ControllerInput input, Delegate activatedDel = null, Delegate deactivatedDel = null, Delegate changedDel = null)
        {
            //Validate that there is something to unregister
            if (activatedDel == null &&
                deactivatedDel == null &&
                changedDel == null)
            {
                Debug.LogErrorFormat("There were no actions to unregister for {0} hand with {1} input", hand, input);
                return;
            }
            if (!_instance.inputLookupDelegation.TryGetValue(hand, out Dictionary<ControllerInput, Delegation> lookup) ||
                !lookup.TryGetValue(input, out Delegation delegation))
            {
                return;
            }

            if (activatedDel != null)
            {
                delegation.OnActivated = Delegate.Remove(delegation.OnActivated, activatedDel);
            }

            if (deactivatedDel != null)
            {
                delegation.OnDeactivated = Delegate.Remove(delegation.OnDeactivated, deactivatedDel);
            }

            if (changedDel != null)
            {
                delegation.OnChanged = Delegate.Remove(delegation.OnChanged, changedDel);
            }
            
            lookup[input] = delegation;
        }
        
        #endregion Unregistration

        #region Processing
        
        private void ProcessInput()
        {
            var leftHandLookup = inputLookupDelegation[Handedness.Left];
            
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

            var rightHandLookup = inputLookupDelegation[Handedness.Right];
            
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
            var isTouch = false;
            
            switch (input)
            {
                case ControllerInput.Start:
                    keyCode = KeyCode.Joystick1Button7;
                    break;
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
                    isTouch = true;
                    break;
                case ControllerInput.TriggerTouch:
                    keyCode = hand == Handedness.Left ? KeyCode.JoystickButton14 : KeyCode.JoystickButton15;
                    isTouch = true;
                    break;
            }

            var delegation = RetrieveDelegation(hand, input);
            if (isTouch)
            {
                var touchChanged = delegation.OnChanged as Action<bool, Handedness>;
                if (touchChanged == null) return;

                var touchResult = Input.GetKey(keyCode);
                touchChanged(touchResult, hand);
            }
            else
            {
                //Check for changed event, always
                var changed = delegation.OnChanged as Action<bool, Handedness>;
                var result = Input.GetKey(keyCode);
                changed?.Invoke(result, hand);

                //Check button press based on condition
                switch (pressCondition)
                {
                    case ButtonCondition.DownPress:
                        if (Input.GetKeyDown(keyCode))
                        {
                            var downActivated = delegation.OnActivated as Action<bool, Handedness>;
                            if (downActivated == null) return;
                            
                            downActivated(true, hand);
                        }
                        else
                        {
                            var downDeactivated = delegation.OnDeactivated as Action<bool, Handedness>;
                            if (downDeactivated == null) return;
                            
                            downDeactivated(true, hand);
                        }
                        break;
                    case ButtonCondition.UpPress:
                        if (Input.GetKeyUp(keyCode))
                        {
                            var upActivated = delegation.OnActivated as Action<bool, Handedness>;
                            if (upActivated == null) return;
                            
                            upActivated(true, hand);
                        }
                        else
                        {
                            var upDeactivated = delegation.OnDeactivated as Action<bool, Handedness>;
                            if (upDeactivated == null) return;
                            
                            upDeactivated(true, hand);
                        }
                        break;
                }
            }
        }

        private void ProcessAxis1D(Handedness hand, ControllerInput input)
        {
            var axisName = string.Empty;

            switch (input)
            {
                case ControllerInput.GripAxis:
                    axisName = hand == Handedness.Left ? "CNFramework_LeftGrip" : "CNFramework_RightGrip";
                    break;
                case ControllerInput.TriggerAxis:
                    axisName = hand == Handedness.Left ? "CNFramework_LeftTrigger" : "CNFramework_RightTrigger";
                    break;
                case ControllerInput.IndexFingerCapacitance:
                    axisName = hand == Handedness.Left ? "CNFramework_LeftIndexCapacitance" : "CNFramework_RightIndexCapacitance";
                    break;
                case ControllerInput.MiddleFingerCapacitance:
                    axisName = hand == Handedness.Left ? "CNFramework_LeftMiddleCapacitance" : "CNFramework_RightMiddleCapacitance";
                    break;
                case ControllerInput.RingFingerCapacitance:
                    axisName = hand == Handedness.Left ? "CNFramework_LeftRingCapacitance" : "CNFramework_RightRingCapacitance";
                    break;
                case ControllerInput.PinkyFingerCapacitance:
                    axisName = hand == Handedness.Left ? "CNFramework_LeftPinkyCapacitance" : "CNFramework_RightPinkyCapacitance";
                    break;
            }

            var result = Input.GetAxis(axisName);
            var delegation = RetrieveDelegation(hand, input);

            if (result - delegation.last1DAxis < Mathf.Epsilon) return;

            var changedAction = delegation.OnChanged as Action<float, Handedness>;
            changedAction?.Invoke(result, hand);

            if (result >= axisTolerance && !delegation.WasActivated)
            {
                delegation.WasActivated = true;
                var activatedAction = delegation.OnActivated as Action<float, Handedness>;
                if (activatedAction == null) return;
                
                activatedAction(result, hand);
            }
            else if (result < axisTolerance && delegation.WasActivated)
            {
                delegation.WasActivated = false;
                var deactivatedAction = delegation.OnDeactivated as Action<float, Handedness>;
                if (deactivatedAction == null) return;
                
                deactivatedAction(result, hand);
            }

            delegation.last1DAxis = result;
        }

        private void ProcessAxis2D(Handedness hand)
        {
            var xAxisName = hand == Handedness.Left ? "CNFramework_LeftHorizontal" : "CNFramework_RightHorizontal";
            var yAxisName = hand == Handedness.Left ? "CNFramework_LeftVertical" : "CNFramework_RightVertical";

            var delegation = RetrieveDelegation(hand, ControllerInput.ThumbStickAxis);
            var action = delegation.OnActivated as Action<Vector2, Handedness>;
            if (action == null) return;

            var axis2d = new Vector2(Input.GetAxis(xAxisName), Input.GetAxis(yAxisName));
            if (axis2d == delegation.last2DAxis) return;
            
            action(axis2d, hand);

            delegation.last2DAxis = axis2d;
        }
        
        #endregion Processing

        private Delegation RetrieveDelegation(Handedness hand, ControllerInput input)
        {
            if (_instance.inputLookupDelegation.TryGetValue(hand, out Dictionary<ControllerInput, Delegation> lookup) &&
                lookup.TryGetValue(input, out Delegation delegation))
            {
                return delegation;
            }

            return null;
        }

        private void ValidateAction(ControllerInput inputUsed, ControllerInput[] validInputs)
        {
            
        }
    }

    //Delegate holder for all state delegations[activated, deactivated, changed]
    public class Delegation
    {
        public Delegate OnActivated;
        public Delegate OnDeactivated;
        public Delegate OnChanged;

        public bool WasActivated;
        public float last1DAxis;
        public Vector2 last2DAxis;
    }

    #region Enumeration
    
    public enum ControllerInput
    {
        /// <summary>Input value used for the Button.Start on OVR input</summary>
        /// Mapped to <see cref="KeyCode.JoystickButton7"/>
        Start,
        /// <summary>Input value most commonly used for the A and X buttons[Menu buttons on Vive].
        /// Mapped to <see cref="KeyCode.JoystickButton0"/>(Right) and <see cref="KeyCode.JoystickButton2"/>(Left) </summary>
        InnerFace,
        /// <summary>Input value most commonly used for the B and Y buttons.
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

    public enum Handedness
    {
        /// <summary>Selection representing the left hand</summary>
        Left,
        /// <summary>Selection representing the right hand</summary>
        Right
    }
    
    #endregion Enumeration

}
