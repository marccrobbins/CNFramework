using CNFramework;
using UnityEngine;

namespace CNFramework
{
    public class OVRDebugHandVisuals : MonoBehaviour
    {
        [SerializeField] private Handedness hand;

        [SerializeField] private GameObject menuButton;
        [SerializeField] private GameObject innerButton;
        [SerializeField] private GameObject outerButton;
        [SerializeField] private GameObject gripButton;
        [SerializeField] private GameObject triggerButton;
        [SerializeField] private GameObject joystickPress;
        [SerializeField] private GameObject joystickTouch;

#if CNFRAMEWORK_DEBUG
    private void Start()
    {
        if (menuButton)
        {
            CNInput.Register(hand, ControllerInput.Start, changedMethod: res => ShowDebug(menuButton, res));
        }

        CNInput.Register(hand, ControllerInput.InnerFace, changedMethod: res => ShowDebug(innerButton, res));
        CNInput.Register(hand, ControllerInput.OuterFace, changedMethod: res => ShowDebug(outerButton, res));
        CNInput.Register(hand, ControllerInput.GripAxis, changedMethod: res => ShowDebug(gripButton, res > 0));
        CNInput.Register(hand, ControllerInput.TriggerAxis, changedMethod: res => ShowDebug(triggerButton, res > 0));
        CNInput.Register(hand, ControllerInput.ThumbStickTouch, changedMethod: res => ShowDebug(joystickTouch, res));
        CNInput.Register(hand, ControllerInput.ThumbStickPress, changedMethod: res => ShowDebug(joystickPress, res));
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
        if (menuButton)
        {
            CNInput.Unregister(hand, ControllerInput.Start, changedMethod: res => ShowDebug(menuButton, res));
        }
        
        CNInput.Unregister(hand, ControllerInput.InnerFace, changedMethod: res => ShowDebug(innerButton, res));
        CNInput.Unregister(hand, ControllerInput.OuterFace, changedMethod: res => ShowDebug(outerButton, res));
        CNInput.Unregister(hand, ControllerInput.GripAxis, changedMethod: res => ShowDebug(gripButton, res > 0));
        CNInput.Unregister(hand, ControllerInput.TriggerAxis, changedMethod: res => ShowDebug(triggerButton, res > 0));
        CNInput.Unregister(hand, ControllerInput.ThumbStickTouch, changedMethod: res => ShowDebug(joystickTouch, res));
        CNInput.Unregister(hand, ControllerInput.ThumbStickPress, changedMethod: res => ShowDebug(joystickPress, res));
    }
#endif
    }
}
