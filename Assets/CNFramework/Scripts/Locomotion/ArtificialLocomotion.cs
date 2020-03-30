using System;
using UnityEngine;

namespace CNFramework
{
    public class ArtificialLocomotion : MonoBehaviour
    {
        public Transform forwardDirection;
        public Transform rigTransform;
        
        [Header("Input")]
        public Axis2D leftAxisInput;
        public Axis2D rightAxisInput;
        public float inputThreshold = 0.2f;

        [Header("Movement")]
        public float moveSpeed = 2;
        public float turnSpeed = 2;

        [Header("Grounding")] 
        public LayerMask validGroundLayers;
        public float gravity = 9.18f;
        public bool isGrounded;
        public float checkDistance = 0.1f;

        private Rigidbody _focusRigidBody;
        private Transform _focusTransform;
        private Transform _forwardTransform;

        private void Start()
        {
            _focusTransform = rigTransform ? rigTransform : transform;
            _focusRigidBody = _focusTransform.GetComponent<Rigidbody>();
            _forwardTransform = forwardDirection ? forwardDirection : transform;
        }

        private void Update()
        {
            MoveRig();
            RotateRig();
        }

        private void LateUpdate()
        {
            UpdateGrounding();
        }

        private void MoveRig()
        {
            var leftAxisValue = leftAxisInput.value;
            if (Mathf.Abs(leftAxisValue.x) <= inputThreshold && 
                Mathf.Abs(leftAxisValue.y) <= inputThreshold) return;

            var right = _forwardTransform.right;
            var forward = _forwardTransform.forward;

            right *= leftAxisValue.x;
            forward *= leftAxisValue.y;

            var heading = right + forward;
            heading.y = 0;
            heading *= moveSpeed;
            
            _focusRigidBody.position += heading * Time.deltaTime;
        }

        private void RotateRig()
        {
            var rightAxisValue = rightAxisInput.value;

            var vector = new Vector3(0, rightAxisValue.x, 0);
            vector *= turnSpeed;
            _focusTransform.localRotation = Quaternion.Euler(_focusTransform.localEulerAngles + vector);
        }

        private void UpdateGrounding()
        {
            var position = _focusTransform.position;
            position.y += checkDistance * 0.5f;
            var ray = new Ray(position, -_focusTransform.up);
            isGrounded = Physics.Raycast(ray, out RaycastHit hit, checkDistance, validGroundLayers);

            if (!isGrounded)
            {
                //fall
                _focusTransform.position -= new Vector3(0, Time.deltaTime * gravity, 0);
            }
        }
    }
}
