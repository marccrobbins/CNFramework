using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNFramework
{
    public class ArtificialLocomotion : MonoBehaviour
    {
        public Transform rigTransform;
        public Axis2D leftAxisInput;
        public Axis2D rightAxisInput;

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
            
            if (leftAxisValue == Vector2.zero) return;
            
            var heading = new Vector3(leftAxisValue.x, 0, leftAxisValue.y);
            heading *= moveSpeed;
            
            rigTransform.Translate(heading * Time.deltaTime);
        }

        private void RotateRig()
        {
            var rightAxisValue = rightAxisInput.value;

            if (rightAxisValue != Vector2.zero)
            {
                var radians = Mathf.Atan2(rightAxisValue.x, rightAxisValue.y);
                var degrees = radians * Mathf.Rad2Deg;

                var rigEuler = rigTransform.eulerAngles;
                rigEuler.y += degrees * Time.deltaTime * turnSpeed;
                rigTransform.eulerAngles = rigEuler;
            }
        }
    }
}
