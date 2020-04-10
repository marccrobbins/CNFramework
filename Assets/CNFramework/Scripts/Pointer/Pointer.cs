using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNFramework
{
    public class Pointer : MonoBehaviour
    {
        [SerializeField] private Handedness hand;
        
        [SerializeField] private Renderer line;
        [SerializeField] private Renderer dot;
        [SerializeField] private float length = 20;
        [SerializeField] private LayerMask validity;

        private void LateUpdate()
        {
            Vector3 originPosition = transform.position;
            Vector3 originForward = transform.forward;

            Ray ray = new Ray(originPosition, originForward);
            bool hasCollided = Physics.Raycast(ray, out RaycastHit hitData, length, validity);

            line.enabled = hasCollided;
            dot.enabled = hasCollided;

            //Get hit position and apply is to dot
            var hitPoint = hasCollided ? hitData.point : originPosition + originForward * length;
            dot.transform.position = hitPoint;

            var distance = Mathf.Abs(Vector3.Distance(transform.position, hitPoint));

            //Adjust line scale
            var lineTransform = line.transform;
            var lineScale = lineTransform.localScale;
            lineScale.z = distance;
            lineTransform.localScale = lineScale;

            //Adjust line position            
            var linePosition = lineTransform.localPosition;
            linePosition.z = lineScale.z * 0.5f;
            lineTransform.localPosition = linePosition;
        }

        private void Select(float result, Handedness handedness)
        {
            Debug.LogFormat("{0} pointer selecting something", hand);
        }
    }
}