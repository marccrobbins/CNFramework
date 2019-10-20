using System;
using UnityEngine;

namespace CNFramework
{
    public class Axis2DAction : InputAction
    {
        [SerializeField] private string xAxisName;
        [SerializeField] private string yAxisName;

        [SerializeField] private Vector2 result;
        
        public event Action<Vector2> OnChange;
       
        protected override void ProcessAction()
        {
            result = new Vector2(Input.GetAxis(xAxisName), Input.GetAxis(yAxisName));
            OnChange?.Invoke(result);
        }
    }
}
