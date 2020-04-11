using UnityEngine;

namespace CNFramework
{
    public class Hand : MonoBehaviour
    {
        private Transform attachPoint;
        [SerializeField] private VelocityEstimator velocityEstimator;

        private Interactable hoveredInteractable;
        private Interactable currentInteractable;
        private bool canGrab;
        
//        private void Grab(float result, Handedness handedness)
//        {
//            if (!hoveredInteractable) return;
//
//            currentInteractable = hoveredInteractable;
//            
//            currentInteractable.Attach(attachPoint ?? transform);
//        }
//
//        private void UnGrab(float result, Handedness handedness)
//        {
//            if (!currentInteractable) return;
//            
//            currentInteractable.Detach(velocityEstimator.Velocity, velocityEstimator.AngularVelocity);
//            currentInteractable = null;
//        }

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
