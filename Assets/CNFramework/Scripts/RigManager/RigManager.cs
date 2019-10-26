using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNFramework
{
    public class RigManager : MonoBehaviour
    {
        public static RigManager Instance;

        [SerializeField] private Transform head;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;

        public Vector3 HeadPosition => head.position;
        public Vector3 LocalHeadPosition => head.localPosition;
        
        public Vector3 LeftHandPosition => leftHand.position;
        public Vector3 LocalLeftHandPosition => leftHand.localPosition;
        
        public Vector3 RightHandPosition => rightHand.position;
        public Vector3 LocalRightHandPosition => rightHand.localPosition;

        private void Awake()
        {
            if (!Instance) Instance = this;
        }
    }
}
