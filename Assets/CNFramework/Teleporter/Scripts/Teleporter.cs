using CNFramework;
using CNFramework.Utility;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private ParabolicPointer pointer;
    [SerializeField] private Transform origin;
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand, rightHand;
    [SerializeField] private Material fadeMaterial;
    [SerializeField] private float teleportFadeDuration;
    
    public TeleportState _state;
    public TeleportState State => _state;

    private Handedness _currentHand;
    private Material _instancedFadeMaterial;
    private float _teleportTimeMarker;
    private Mesh _planeMesh;
    private bool _isFadeIn;
    private int _fadeID;

    #region MonoHehaviour

    private void Start()
    {
        CNInput.Register(Handedness.Right, ControllerInput.ThumbStickTouch, changedMethod: SetVisibility);
        CNInput.Register(Handedness.Right, ControllerInput.ThumbStickAxis, SetAxis);
        CNInput.Register(Handedness.Right, ControllerInput.ThumbStickPress, Teleport);
        
        CNInput.Register(Handedness.Left, ControllerInput.ThumbStickTouch, changedMethod: SetVisibility);
        CNInput.Register(Handedness.Left, ControllerInput.ThumbStickAxis, SetAxis);
        CNInput.Register(Handedness.Left, ControllerInput.ThumbStickPress, Teleport);

        if (fadeMaterial != null)
        {
            _instancedFadeMaterial = Instantiate(fadeMaterial);
        }
        
        _planeMesh = new Mesh();
        var vertices = new []
        {
            new Vector3(-1, -1, 0),
            new Vector3(-1, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, -1, 0)
        };
        var triangles = new [] { 0, 1, 2, 0, 2, 3 };
        _planeMesh.vertices = vertices;
        _planeMesh.triangles = triangles;
        _planeMesh.RecalculateBounds();

        _fadeID = Shader.PropertyToID("_Fade");
    }

    private void Update()
    {
        if (_state != TeleportState.Teleporting) return;

        if (Time.time - _teleportTimeMarker >= teleportFadeDuration * 0.5f)
        {
            if (_isFadeIn)
            {
                _state = TeleportState.None;
            }
            else
            {
                Vector3 offset = origin.position - head.position;
                offset.y = 0;
                origin.position = pointer.SelectedPoint + offset;
            }

            _teleportTimeMarker = Time.time;
            _isFadeIn = !_isFadeIn;
        }
    }

    private void OnPostRender()
    {
        if (_state != TeleportState.Teleporting) return;

        var alpha = Mathf.Clamp01((Time.time - _teleportTimeMarker) / (teleportFadeDuration * 0.5f));
        if (_isFadeIn)
        {
            alpha = 1 - alpha;
        }

        var localMatrix = Matrix4x4.TRS(Vector3.forward * 0.3f, Quaternion.identity, Vector3.one);
        _instancedFadeMaterial.SetPass(0);
        _instancedFadeMaterial.SetFloat(_fadeID, alpha);
        Graphics.DrawMeshNow(_planeMesh, transform.localToWorldMatrix * localMatrix);
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
        if(_state == TeleportState.Teleporting ||
           !pointer.PointOnNavMesh) return;

        _state = TeleportState.Teleporting;
        
//        Vector3 offset = origin.position - head.position;
//        offset.y = 0;
//        origin.position = pointer.SelectedPoint + offset;
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
