using System.Collections;
using OculusSampleFramework;
using UnityEngine;

public class FingerTipFollow : MonoBehaviour
{
    public OVRPlugin.HandFinger fingerToFollow;

    private bool _isInitialized;
    private HandsManager _handManager;
    private OVRSkeleton _handSkeleton;
    private OVRHand _hand;
    private OVRSkeleton.BoneId _boneId;
    private OVRBoneCapsule _capsuleToTrack;
    
    private IEnumerator Start()
    {
        while (!HandsManager.Instance || !HandsManager.Instance.IsInitialized())
        {
            yield return null;
        }

        _handManager = HandsManager.Instance;
        _handSkeleton = GetComponentInParent<OVRSkeleton>();
        _hand = GetComponentInParent<OVRHand>();
        
        if(!_handSkeleton || !_hand) yield break;

        switch (fingerToFollow)
        {
            case OVRPlugin.HandFinger.Thumb:
                _boneId = OVRSkeleton.BoneId.Hand_Thumb3;
                break;
            case OVRPlugin.HandFinger.Index:
                _boneId = OVRSkeleton.BoneId.Hand_Index3;
                break;
            case OVRPlugin.HandFinger.Middle:
                _boneId = OVRSkeleton.BoneId.Hand_Middle3;
                break;
            case OVRPlugin.HandFinger.Ring:
                _boneId = OVRSkeleton.BoneId.Hand_Ring3;
                break;
            case OVRPlugin.HandFinger.Pinky:
                _boneId = OVRSkeleton.BoneId.Hand_Pinky3;
                break;
            default:
                _boneId = OVRSkeleton.BoneId.Hand_Index3;
                break;
        }
        
//        var capsuleTriggers = new List<BoneCapsuleTriggerLogic>();
        var boneCapsules = HandsManager.GetCapsulesPerBone(_handSkeleton, _boneId);
//        foreach (var capsule in boneCapsules)
//        {
//            var capsuleTrigger = capsule.CapsuleRigidbody.gameObject.AddComponent<BoneCapsuleTriggerLogic>();
//            capsule.CapsuleCollider.isTrigger = true;
//            capsuleTriggers.Add(capsuleTrigger);
//        }

        if (boneCapsules.Count > 0)
        {
            _capsuleToTrack = boneCapsules[0];
        }

        _isInitialized = true;
    }

    private void Update()
    {
        if(!_isInitialized || _capsuleToTrack == null) return;

        var capsuleTransform = _capsuleToTrack.CapsuleCollider.transform;
        var capsuleDirection = capsuleTransform.right;
        var trackedPosition = capsuleTransform.position + _capsuleToTrack.CapsuleCollider.height * 0.5f * capsuleDirection;
        var sphereRadiusOffset = _hand.HandScale * (transform.localScale.z * 0.5f) * capsuleDirection;
        transform.position = trackedPosition + sphereRadiusOffset;
    }
}
