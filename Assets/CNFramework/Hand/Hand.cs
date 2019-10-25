using UnityEngine;

namespace CNFramework
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] private Handedness hand;
        [SerializeField] private Transform attachPoint;
        [SerializeField] private VelocityEstimator velocityEstimator;

        private Interactable hoveredInteractable;
        private Interactable currentInteractable;
        private bool canGrab;
        
        private void Start()
        {
            CNInput.Register(hand, ControllerInput.GripAxis, Grab, UnGrab);
        }

        private void OnDisable()
        {
            CNInput.Unregister(hand, ControllerInput.GripAxis, UnGrab, UnGrab);
        }

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
