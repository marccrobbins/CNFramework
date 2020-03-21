using System;
using UnityEngine;

namespace CNFramework
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private bool attachToPoint;

        private bool isAttached;
        private Rigidbody rigidbody;
        private Vector3 attachPosition;
        
        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            attachPosition = transform.position;
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
            
            rigidbody.velocity = velocity;
            rigidbody.angularVelocity = angular;
            
            isAttached = false;
        }
    }
}
