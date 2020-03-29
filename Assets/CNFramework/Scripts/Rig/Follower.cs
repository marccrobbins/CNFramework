using UnityEngine;

namespace CNFramework
{
    public class Follower : MonoBehaviour
    {
        public bool isLocal;
        public Transform source;
        public Transform target;
        public float damping = 1;

        [EnumFlags] public AxisFlags transformAxis;
        [EnumFlags] public AxisFlags eulerAxis;

        private void Awake()
        {
            if (!source)
            {
                Debug.LogWarningFormat("There is no Source reference for {0} follower", name);
            }
            
            if (!target)
            {
                Debug.LogWarningFormat("There is no Target reference for {0} follower", name);
            }
        }

        private void FixedUpdate()
        {
            if (!source || !target) return;

            if (isLocal)
            {
                var targetPosition = LockAxis(source.localPosition, transformAxis);
                var targetRotation = LockAxis(source.localEulerAngles, eulerAxis);
                
                target.localPosition = LockAxis(source.localPosition, transformAxis);
                target.localEulerAngles = LockAxis(source.localEulerAngles, eulerAxis);
            }
            else
            {
                target.position = LockAxis(source.position, transformAxis);
                target.eulerAngles = LockAxis(source.eulerAngles, eulerAxis);
            }
        }

        private Vector3 LockAxis(Vector3 original, AxisFlags axis)
        {
            var result = original;
            
            if (!axis.HasFlag(AxisFlags.X)) result.x = 0;
            if (!axis.HasFlag(AxisFlags.Y)) result.y = 0;
            if (!axis.HasFlag(AxisFlags.Z)) result.z = 0;

            return result;
        }
        
//        private Quaternion LockQuaternionAxis(Quaternion original)
//        {
//            var result = original;
//            
//            if (!eulerAxis.HasFlag(QuaternionAxisFlags.X)) result.x = 0;
//            if (!eulerAxis.HasFlag(QuaternionAxisFlags.Y)) result.y = 0;
//            if (!eulerAxis.HasFlag(QuaternionAxisFlags.Z)) result.z = 0;
//            if (!eulerAxis.HasFlag(QuaternionAxisFlags.W)) result.w = 0;
//
//            return result;
//        }
    }
}
