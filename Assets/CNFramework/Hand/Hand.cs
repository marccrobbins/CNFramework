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
            switch (hand)
            {
                case Handedness.Left:
                    RigManager.Instance.LeftHandInput.GripAxis.OnActivated += Grab;
                    RigManager.Instance.LeftHandInput.GripAxis.OnDeactivated += UnGrab;
                    break;
                case Handedness.Right:
                    RigManager.Instance.RightHandInput.GripAxis.OnActivated += Grab;
                    RigManager.Instance.RightHandInput.GripAxis.OnDeactivated += UnGrab;
                    break;
            }
        }

        private void OnDisable()
        {
            switch (hand)
            {
                case Handedness.Left:
                    RigManager.Instance.LeftHandInput.GripAxis.OnActivated -= Grab;
                    RigManager.Instance.LeftHandInput.GripAxis.OnDeactivated -= UnGrab;
                    break;
                case Handedness.Right:
                    RigManager.Instance.RightHandInput.GripAxis.OnActivated -= Grab;
                    RigManager.Instance.RightHandInput.GripAxis.OnDeactivated -= UnGrab;
                    break;
            }
        }

        private void Grab()
        {
            if (!hoveredInteractable) return;

            currentInteractable = hoveredInteractable;
            
            currentInteractable.Attach(attachPoint ?? transform);
        }

        private void UnGrab()
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

    public enum Handedness
    {
        Left,
        Right
    }
}
