using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

namespace CNFramework
{
    public class RigManager : MonoBehaviour
    {
        public static RigManager Instance;

        [SerializeField] private Transform head;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;

        //World transform
        public Vector3 HeadPosition => head.position;
        public Vector3 LeftHandPosition => leftHand.position;
        public Vector3 RightHandPosition => rightHand.position;
        
        public Quaternion HeadRotation => head.rotation;
        public Quaternion LeftHandRotation  => leftHand.rotation;
        public Quaternion RightHandRotation  => rightHand.rotation;
        
        public Vector3 HeadScale  => head.lossyScale;
        public Vector3 LeftHandScale  => leftHand.lossyScale;
        public Vector3 RightHandScale => rightHand.lossyScale;

        //Local Transform
        public Vector3 LocalHeadPosition => head.localPosition;
        public Vector3 LocalLeftHandPosition => leftHand.localPosition;
        public Vector3 LocalRightHandPosition => rightHand.localPosition;
        
        public Quaternion LocalHeadRotation => head.localRotation;
        public Quaternion LocalLeftHandRotation  => leftHand.localRotation;
        public Quaternion LocalRightHandRotation  => rightHand.localRotation;
        
        public Vector3 LocalHeadScale  => head.localScale;
        public Vector3 LocalLeftHandScale  => leftHand.localScale;
        public Vector3 LocalRightHandScale => rightHand.localScale;

        public TrackedPoseDriver driver;
        public Vector3 poseOffset;

        private void Awake()
        {
            if (!Instance) Instance = this;
        }

        private void Update()
        {
            var pose = new Pose();
            pose.position = poseOffset;
            
            driver.originPose = pose;
        }
    }
}
