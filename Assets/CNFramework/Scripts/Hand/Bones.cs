using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bones : MonoBehaviour
{
    public GameObject boneFollowerPrefab;
    public OVRSkeleton skeleton;

    private bool _isInitialized;
    private List<OVRBone> _bones;
    private List<Transform> _boneFollowers = new List<Transform>();
     
    public IEnumerator Start()
    {
        while (!skeleton.IsInitialized)
        {
            yield return null;
        }

        _bones = new List<OVRBone>(skeleton.Bones);

        if (_bones == null ||
            _bones.Count == 0)
        {
            Debug.LogFormat("skeleton bones : {0}", skeleton.Bones.Count);
            yield break;
        }

        foreach (var bone in _bones)
        {
            _boneFollowers.Add(Instantiate(boneFollowerPrefab, bone.Transform.position, Quaternion.identity, transform).transform);
        }

        _isInitialized = true;
    }

    private void LateUpdate()
    {
        if (!_isInitialized) return;
        
        for (var i = 0; i < _bones.Count; i++)
        {
            if (i > _boneFollowers.Count) break;

            var follower = _boneFollowers[i];
            follower.transform.position = _bones[i].Transform.position;
        }
    }
}
