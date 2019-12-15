using CNFramework;
using CNFramework.Utility;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private ParabolicPointer pointer;
    [SerializeField] private Transform origin;
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand, rightHand;
    
    public TeleportState _state;
    public TeleportState State => _state;

    private Handedness _currentHand;

    #region MonoHehaviour

    private void Start()
    {
        CNInput.Register(Handedness.Right, ControllerInput.ThumbStickTouch, changedMethod: SetVisibility);
        CNInput.Register(Handedness.Right, ControllerInput.ThumbStickAxis, SetAxis);
        CNInput.Register(Handedness.Right, ControllerInput.ThumbStickPress, Teleport);
        
        CNInput.Register(Handedness.Left, ControllerInput.ThumbStickTouch, changedMethod: SetVisibility);
        CNInput.Register(Handedness.Left, ControllerInput.ThumbStickAxis, SetAxis);
        CNInput.Register(Handedness.Left, ControllerInput.ThumbStickPress, Teleport);
    }

    private void OnDisable()
    {
        CNInput.Unregister(Handedness.Right, ControllerInput.ThumbStickTouch, changedMethod: SetVisibility);
        CNInput.Unregister(Handedness.Right, ControllerInput.ThumbStickAxis, SetAxis);
        CNInput.Unregister(Handedness.Right, ControllerInput.ThumbStickPress, Teleport);
        
        CNInput.Unregister(Handedness.Left, ControllerInput.ThumbStickTouch, changedMethod: SetVisibility);
        CNInput.Unregister(Handedness.Left, ControllerInput.ThumbStickAxis, SetAxis);
        CNInput.Unregister(Handedness.Left, ControllerInput.ThumbStickPress, Teleport);
    }

    #endregion MonoBehaviour

    private void SetVisibility(bool isOn, Handedness handedness)
    {
        if (_state == TeleportState.Teleporting || 
            _state == TeleportState.Selecting && 
            handedness != _currentHand) return;
        
        _currentHand = handedness;

        pointer.enabled = isOn;
        pointer.Origin = handedness == Handedness.Left ? leftHand : rightHand;
        _state = isOn ? TeleportState.Selecting : TeleportState.None;
    }

    private float angle;
    public float adjustment;
    private void SetAxis(Vector2 axis, Handedness handedness)
    {
        if(_state != TeleportState.Selecting ||
           handedness != _currentHand) return;

        var atan = Mathf.Atan2(axis.x, axis.y);
        var deg = atan * Mathf.Rad2Deg;
        
        pointer.SelectionAngle = deg + origin.localEulerAngles.y;
        
        Debug.LogFormat("axis : {0} | atan : {1} | deg : {2} | hand : {3}", axis, atan, deg, handedness);
    }

    private void Teleport(bool shouldTeleport, Handedness handedness)
    {
        if(_state == TeleportState.Teleporting ||
           !pointer.PointOnNavMesh) return;

        _state = TeleportState.Teleporting;
        
        //Store Values
        var newPosition = pointer.SelectedPoint;
        var newAngle = pointer.SelectionAngle;
        
        Debug.Log(newAngle);
        
        Fader.Blink(() =>
        {
            //Apply position
            Vector3 offset = origin.position - head.position;
            offset.y = 0;
            origin.position = newPosition + offset;
                
//            //Apply rotation
//            var rot = origin.localEulerAngles;
//            rot.y = newAngle;
//            origin.localEulerAngles = rot;

            _state = TeleportState.None;
        });
    }
}

public enum TeleportState
{
    /// The player is not using teleportation right now
    None,
    /// The player is currently selecting a teleport destination (holding down on touchpad)
    Selecting,
    /// The player has selected a teleport destination and is currently teleporting now (fading in/out)
    Teleporting
}
