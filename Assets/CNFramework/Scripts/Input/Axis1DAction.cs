using System;
using UnityEngine;

namespace CNFramework
{
    public class Axis1DAction : InputAction
    {
        [SerializeField] private string axisName;
        [SerializeField] [Range(0, 1)] private float tolerance = 0.75f;
        [SerializeField] private float result;

        public event Action OnActivated;
        public event Action OnDeactivated;
        public event Action<float> OnChanged;

        private bool wasActivated;
        
        protected override void ProcessAction()
        {
            result = Input.GetAxis(axisName);
            
            OnChanged?.Invoke(result);
            
            if (!wasActivated && result >= tolerance)
            {
                OnActivated?.Invoke();
                wasActivated = true;
            }
            else if (wasActivated && result < tolerance)
            {
                OnDeactivated?.Invoke();
                wasActivated = false;
            }
        }
    }
}
