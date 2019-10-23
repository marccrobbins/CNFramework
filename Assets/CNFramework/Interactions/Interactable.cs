using System;
using UnityEngine;

namespace CNFramework
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private InteractableType interactableType;
        [SerializeField] private bool attachToPoint;

        private bool isAttached;
        private Rigidbody rigidbody;
        private Vector3 attachPosition;
        
        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            attachPosition = transform.position;
        }

        private void Update()
        {
            switch (interactableType)
            {
                case InteractableType.Spring:
                    if (isAttached) break;
                    
                    var distance = Vector3.Distance(transform.position, attachPosition);
                    if (distance > 0.001f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, attachPosition, 5 * Time.deltaTime);
                    }
                    break;
            }
        }

        public void Attach(Transform attachPoint, bool worldPositionStays = true)
        {
            transform.SetParent(attachPoint, worldPositionStays);
            if (attachToPoint)
            {
                transform.localPosition = Vector3.zero;
            }
            
            rigidbody.isKinematic = true;
            isAttached = true;
        }

        public void Detach(Vector3 velocity, Vector3 angular)
        {
            transform.SetParent(null);
            rigidbody.isKinematic = false;
            
            switch (interactableType)
            {
                case InteractableType.Normal:
                    rigidbody.velocity = velocity;
                    rigidbody.angularVelocity = angular;
                    break;
            }
            
            isAttached = false;
        }
    }

    public enum InteractableType
    {
        Normal = 0,
        Spring
    }
}
