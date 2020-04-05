using UnityEngine;

namespace CNFramework
{
    [RequireComponent(typeof(Rigidbody))]
    public class VelocityLimiter : MonoBehaviour
    {
        public LayerMask limitLayers;

        private Rigidbody _rigidBody;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void OnCollisionStay(Collision other)
        {
            if (limitLayers == (limitLayers | (1 << other.gameObject.layer)))
            {
                _rigidBody.velocity = Vector3.zero;
            }
        }
    }
}
