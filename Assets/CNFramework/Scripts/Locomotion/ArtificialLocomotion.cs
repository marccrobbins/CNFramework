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

        private void Update()
        {
            MoveRig();
            RotateRig();
        }

        private void LateUpdate()
        {
            //UpdateGrounding();
        }

        private void MoveRig()
        {
            var leftAxisValue = leftAxisInput.value;
            if (Mathf.Abs(leftAxisValue.x) <= inputThreshold && 
                Mathf.Abs(leftAxisValue.y) <= inputThreshold) return;

            var right = forwardDirection.right * leftAxisValue.x;
            var forward = forwardDirection.forward * leftAxisValue.y;

            var heading = right + forward;
            heading.y = 0;
            heading *= moveSpeed;

            rigTransform.position += heading * Time.deltaTime;
        }

        private void RotateRig()
        {
            var rightAxisValue = rightAxisInput.value;

            var vector = new Vector3(0, rightAxisValue.x, 0);
            vector *= turnSpeed;
            rigTransform.localRotation = Quaternion.Euler(rigTransform.localEulerAngles + vector);
        }

        private void UpdateGrounding()
        {
            var position = rigTransform.position;
            position.y += checkDistance * 0.5f;
            var ray = new Ray(position, -rigTransform.up);
            Debug.DrawRay(position, -rigTransform.up, Color.red, checkDistance);
            isGrounded = Physics.Raycast(ray, out RaycastHit hit, checkDistance, validGroundLayers);

            if (isGrounded)
            {
                var rigPosition = rigTransform.position;
                rigPosition.y = hit.point.y;
                rigTransform.position = rigPosition;
            }
            else
            {
                //fall
                rigTransform.position -= new Vector3(0, Time.deltaTime * gravity, 0);
            }
        }
    }
}
