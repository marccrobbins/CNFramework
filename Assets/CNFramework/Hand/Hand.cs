using UnityEngine;

namespace CNFramework
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] private Handedness hand;
        [SerializeField] private Transform attachPoint;
        [SerializeField] private VelocityEstimator velocityEstimator;
        
        private Vector3 lastPosition;
        private Vector3 lastEuler;
        private Interactable hoveredInteractable;
        private Interactable currentInteractable;
        private bool canGrab;
        
        private void Start()
        {
            CNInput.Register(hand, ControllerInput.GripAxis, Grab);
            CNInput.Register(hand, ControllerInput.InnerFace, TestMenu);
        }

        void TestMenu(bool result)
        {
            Debug.LogFormat("Menu button pressed: {0}", result);
        }

        private void OnDisable()
        {
            CNInput.Unregister(hand, ControllerInput.GripAxis, UnGrab);
            CNInput.Unregister(hand, ControllerInput.InnerFace, TestMenu);
        }

        private void Grab(float result)
        {
            Debug.Log("Grab");
            if (!hoveredInteractable) return;

            currentInteractable = hoveredInteractable;
            
            currentInteractable.Attach(attachPoint ?? transform);
        }

        private void UnGrab(float result)
        {
            Debug.Log("Ungrab");
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
