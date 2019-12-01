using System;
using CNFramework;
using CNFramework.Utility;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Hand _hand;
    [SerializeField] private ParabolicPointer _pointer;
    
    private TeleportState _state;
    public TeleportState State => _state;

    private void Start()
    {
        CNInput.Register(_hand.Handedness, ControllerInput.ThumbStickTouch, changedMethod: SetVisibility);
        CNInput.Register(_hand.Handedness, ControllerInput.ThumbStickAxis, SetAxis);
        CNInput.Register(_hand.Handedness, ControllerInput.ThumbStickPress, Teleport);
    }

    private void SetVisibility(bool isOn)
    {
        _state = isOn ? TeleportState.Selecting : TeleportState.None;
        _pointer.enabled = isOn;
    }

    private void SetAxis(Vector2 axis)
    {
        //This will control which direction the player is facing when teleport happens
        if(_state != TeleportState.Selecting) return;
    }

    private void Teleport(bool shouldTeleport)
    {
        _state = TeleportState.Teleporting;
        //Do fade out
        Fader.FadeOut(() =>
        {
            //Change positions
            //Then fade back in
            Fader.FadeIn(() =>
            {
                Debug.Log("TeleportComplete");
            });
        });
    }

    private void OnDisable()
    {
        CNInput.Unregister(_hand.Handedness, ControllerInput.ThumbStickTouch, changedMethod: SetVisibility);
        CNInput.Unregister(_hand.Handedness, ControllerInput.ThumbStickAxis, SetAxis);
        CNInput.Unregister(_hand.Handedness, ControllerInput.ThumbStickPress, Teleport);
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
