using System;
using UnityEngine;

namespace CNFramework
{
    public class Axis1D : InputAction<float>
    {
        public string axisName;
        
        private void Start()
        {
            if (string.IsNullOrEmpty(axisName))
            {
                Debug.LogError("There is no value entered for axisName");
                return;
            }

            IsValid = true;
        }

        private void Update()
        {
            if(!IsValid) return;

            value = Input.GetAxis(axisName);
            
            ProcessValue(value);
        }
    }
}
