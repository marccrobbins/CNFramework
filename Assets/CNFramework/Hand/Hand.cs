using UnityEngine;

namespace CNFramework
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] private Handedness hand;
        [SerializeField] private Transform attachPoint;
        [SerializeField] private VelocityEstimator velocityEstimator;

        [Header("Debug")] 
        [SerializeField] private GameObject dpad;
        [SerializeField] private GameObject touchSphere;
        [SerializeField] private GameObject gripSpheres;
        [SerializeField] private GameObject menuSphere;
        [SerializeField] private GameObject triggerSphere;
        
        private Vector3 lastPosition;
        private Vector3 lastEuler;
        private Interactable hoveredInteractable;
        private Interactable currentInteractable;
        private bool canGrab;
        
        private void Start()
        {
            CNInput.Register(hand, ControllerInput.GripAxis, Grab, UnGrab);
            
            //Debug
            CNInput.Register(hand, ControllerInput.ThumbStickPress, changedMethod: (res) => ShowDebug(dpad, res));
            CNInput.Register(hand, ControllerInput.ThumbStickAxis, SetTouchPosition);
            CNInput.Register(hand, ControllerInput.ThumbStickTouch, changedMethod: (res) => ShowDebug(touchSphere, res));
            CNInput.Register(hand,ControllerInput.TriggerTouch, changedMethod: (res) => ShowDebug(triggerSphere, res));
            CNInput.Register(hand, ControllerInput.GripAxis, changedMethod: (res) => ShowDebug(gripSpheres, res > 0));
            CNInput.Register(hand, ControllerInput.InnerFace, changedMethod: (res) => ShowDebug(menuSphere, res));
        }

        private void OnDisable()
        {
            CNInput.Unregister(hand, ControllerInput.GripAxis, UnGrab, UnGrab);
            
            //Debug
            CNInput.Unregister(hand, ControllerInput.ThumbStickPress, changedMethod: (res) => ShowDebug(dpad, res));
            CNInput.Unregister(hand, ControllerInput.ThumbStickAxis, SetTouchPosition);
            CNInput.Unregister(hand, ControllerInput.ThumbStickTouch, changedMethod: (res) => ShowDebug(touchSphere, res));
            CNInput.Unregister(hand, ControllerInput.TriggerTouch, changedMethod: (res) => ShowDebug(triggerSphere, res));
            CNInput.Unregister(hand, ControllerInput.GripAxis, changedMethod: (res) => ShowDebug(gripSpheres, res > 0));
            CNInput.Unregister(hand, ControllerInput.InnerFace, changedMethod: (res) => ShowDebug(menuSphere, res));
        }

        #region Debug

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
        
        #endregion Debug


        private void Grab(float result)
        {
            if (!hoveredInteractable) return;

            currentInteractable = hoveredInteractable;
            
            currentInteractable.Attach(attachPoint ?? transform);
        }

        private void UnGrab(float result)
        {
            if (!currentInteractable) return;
            
            currentInteractable.Detach(velocityEstimator.Velocity, velocityEstimator.AngularVelocity);
            currentInteractable = null;
        }

        private void OnTriggerStay(Collider other)
        {
            hoveredInteractable = other.GetComponent<Interactable>();
        }

        private void OnTriggerExit(Collider other)
        {
            hoveredInteractable = null;
        }
    }

   
}
