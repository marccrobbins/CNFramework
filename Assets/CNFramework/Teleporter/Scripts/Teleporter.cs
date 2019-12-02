using CNFramework;
using CNFramework.Utility;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform origin;
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand, rightHand;
    [SerializeField] private ParabolicPointer pointer;
    
    private TeleportState _state;
    public TeleportState State => _state;

    private Handedness _currentHand;

    private void Start()
    {
        CNInput.Register(Handedness.Right, ControllerInput.ThumbStickTouch, changedMethod: SetVisibility);
        CNInput.Register(Handedness.Right, ControllerInput.ThumbStickAxis, SetAxis);
        CNInput.Register(Handedness.Right, ControllerInput.ThumbStickPress, Teleport);
        
        CNInput.Register(Handedness.Left, ControllerInput.ThumbStickTouch, changedMethod: SetVisibility);
        CNInput.Register(Handedness.Left, ControllerInput.ThumbStickAxis, SetAxis);
        CNInput.Register(Handedness.Left, ControllerInput.ThumbStickPress, Teleport);
    }

    private void SetVisibility(bool isOn, Handedness handedness)
    {
        if (_state == TeleportState.Selecting && 
            _currentHand != handedness) return;
        
        _currentHand = handedness;

        pointer.enabled = isOn;
        pointer.Origin = handedness == Handedness.Left ? leftHand : rightHand;
        _state = isOn ? TeleportState.Selecting : TeleportState.None;
    }

    private void SetAxis(Vector2 axis, Handedness handedness)
    {
        //This will control which direction the player is facing when teleport happens
        if(_state != TeleportState.Selecting) return;
    }

    private void Teleport(bool shouldTeleport, Handedness handedness)
    {
        if(!pointer.PointOnNavMesh) return;

        _state = TeleportState.Teleporting;
        
        Vector3 offset = origin.position - head.position;
        offset.y = 0;
        origin.position = pointer.SelectedPoint + offset;
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
