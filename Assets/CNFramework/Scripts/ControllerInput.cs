using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerInput : MonoBehaviour
{
    [SerializeField] private SteamVR_Input_Sources inputSource;
    [SerializeField] private SteamVR_Action_Boolean trigger;

    [SerializeField] private bool isPressed;

    private void Start()
    {
        trigger.AddOnChangeListener(OnTriggerChange, inputSource);
    }

    private void OnTriggerChange(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool result)
    {
        Debug.LogFormat("{0} trigger changed.", fromSource);
        isPressed = result;
    }
}
