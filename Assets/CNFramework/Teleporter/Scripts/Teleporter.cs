using CNFramework;
using CNFramework.Utility;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform origin;
    [SerializeField] private Transform head;
    [SerializeField] private Transform hand;
    [SerializeField] private ParabolicPointer pointer;
    
    private TeleportState _state;
    public TeleportState State => _state;

    private void Start()
    {
        CNInput.Register(Handedness.Right, ControllerInput.ThumbStickTouch, changedMethod: SetVisibility);
        CNInput.Register(Handedness.Right, ControllerInput.ThumbStickAxis, SetAxis);
        CNInput.Register(Handedness.Right, ControllerInput.ThumbStickPress, Teleport);

        pointer.Origin = hand;
    }

    private void SetVisibility(bool isOn)
    {
        _state = isOn ? TeleportState.Selecting : TeleportState.None;
        pointer.enabled = isOn;
    }

    private void SetAxis(Vector2 axis)
    {
        //This will control which direction the player is facing when teleport happens
        if(_state != TeleportState.Selecting) return;
    }

    private void Teleport(bool shouldTeleport)
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
