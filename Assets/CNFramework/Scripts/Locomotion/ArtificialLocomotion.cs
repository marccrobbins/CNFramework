using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNFramework
{
    public class ArtificialLocomotion : MonoBehaviour
    {
        public Transform forwardDirection;
        public Transform rigTransform;
        public Axis2D leftAxisInput;
        public Axis2D rightAxisInput;
        public float inputThreshold = 0.2f;

        public float moveSpeed = 2;
        public float turnSpeed = 2;

        private void Update()
        {
            MoveRig();
            RotateRig();
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
    }
}
