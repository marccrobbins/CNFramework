using System;
using UnityEngine;

namespace CNFramework
{
    public class Axis2D : InputAction<Vector2>
    {
        public string axisNameX;
        public string axisNameY;

        private void Start()
        {
            if (string.IsNullOrEmpty(axisNameX))
            {
                Debug.LogError("There is no value entered for axisNameX");
                return;
            }
            
            if (string.IsNullOrEmpty(axisNameY))
            {
                Debug.LogError("There is no value entered for axisNameY");
                return;
            }

            IsValid = true;
            
            value = Vector2.zero;
        }

        private void Update()
        {
            if(!IsValid) return;

            var xValue = Input.GetAxis(axisNameX);
            var yValue = Input.GetAxis(axisNameY);

            value = new Vector2(xValue, yValue);

            ProcessValue(value);
        }
    }
}
