using UnityEngine;

namespace CNFramework
{
    public class Follower : MonoBehaviour
    {
        public bool isLocal;
        public Transform source;
        public Transform target;
        public Vector3 positionOffset;
        public Vector3 eulerOffset;

        [EnumFlags] public AxisFlags transformAxis;
        [EnumFlags] public AxisFlags eulerAxis;

        private Vector3 _startingPosition;
        private Vector3 _startingEuler;
        
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

        private void LateUpdate()
        {
            if (!source || !target) return;

            if (isLocal)
            {
                target.localPosition = LockAxis(target.localPosition, source.localPosition, transformAxis) + positionOffset;
                target.localEulerAngles = LockAxis(target.localEulerAngles, source.localEulerAngles, eulerAxis) + eulerOffset;
            }
            else
            {
                target.position = LockAxis(target.position, source.position, transformAxis) + positionOffset;
                target.eulerAngles = LockAxis(target.eulerAngles, source.eulerAngles, eulerAxis) + eulerOffset;
            }
        }

        private Vector3 LockAxis(Vector3 original, Vector3 changed, AxisFlags axis)
        {
            var result = original;
            var axisIndex = (int) axis;

            switch (axisIndex)
            {
                case -1 :
                    result = changed;
                    break;
                case 1:
                    result.x = changed.x;
                    break;
                case 2:
                    result.y = changed.y;
                    break;
                case 3:
                    result.x = changed.x;
                    result.y = changed.y;
                    break;
                case 4:
                    result.z = changed.z;
                    break;
                case 5:
                    result.x = changed.x;
                    result.z = changed.z;
                    break;
                case 6:
                    result.y = changed.y;
                    result.z = changed.z;
                    break;
            }

            return result;
        }
    }
}
