using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNFramework
{
    public class DebugHandVisuals : MonoBehaviour
    {
        [SerializeField] private Handedness hand;
        
        [SerializeField] private GameObject dpad;
        [SerializeField] private GameObject touchSphere;
        [SerializeField] private GameObject gripSpheres;
        [SerializeField] private GameObject menuSphere;
        [SerializeField] private GameObject triggerSphere;
        
        private void Start()
        {
            CNInput.Register(hand, ControllerInput.ThumbStickPress, changedMethod: (res) => ShowDebug(dpad, res));
            CNInput.Register(hand, ControllerInput.ThumbStickAxis, SetTouchPosition);
            CNInput.Register(hand, ControllerInput.ThumbStickTouch, changedMethod: (res) => ShowDebug(touchSphere, res));
            CNInput.Register(hand,ControllerInput.TriggerTouch, changedMethod: (res) => ShowDebug(triggerSphere, res));
            CNInput.Register(hand, ControllerInput.GripAxis, changedMethod: (res) => ShowDebug(gripSpheres, res > 0));
            CNInput.Register(hand, ControllerInput.InnerFace, changedMethod: (res) => ShowDebug(menuSphere, res));
        }
        
        private void SetTouchPosition(Vector2 position)
        {
            if (!touchSphere.activeInHierarchy) return;
            
            touchSphere.transform.localPosition = new Vector3(position.x, 0, -position.y);
        }
        
        private void ShowDebug(GameObject debugObject, bool isActive)
        {
            if (isActive && !debugObject.activeInHierarchy)
            {
                debugObject.SetActive(true);
            }
            else if (!isActive && debugObject.activeInHierarchy)
            {
                debugObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            CNInput.Unregister(hand, ControllerInput.ThumbStickPress, changedMethod: (res) => ShowDebug(dpad, res));
            CNInput.Unregister(hand, ControllerInput.ThumbStickAxis, SetTouchPosition);
            CNInput.Unregister(hand, ControllerInput.ThumbStickTouch, changedMethod: (res) => ShowDebug(touchSphere, res));
            CNInput.Unregister(hand, ControllerInput.TriggerTouch, changedMethod: (res) => ShowDebug(triggerSphere, res));
            CNInput.Unregister(hand, ControllerInput.GripAxis, changedMethod: (res) => ShowDebug(gripSpheres, res > 0));
            CNInput.Unregister(hand, ControllerInput.InnerFace, changedMethod: (res) => ShowDebug(menuSphere, res));
        }
    }
}
