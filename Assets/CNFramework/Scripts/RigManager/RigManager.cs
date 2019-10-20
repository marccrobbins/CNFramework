using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNFramework
{
    public class RigManager : MonoBehaviour
    {
        public static RigManager Instance;

        [SerializeField] private ControllerInput leftHandInput;
        public ControllerInput LeftHandInput => leftHandInput;

        [SerializeField] private ControllerInput rightHandInput;
        public ControllerInput RightHandInput => rightHandInput;

        private void Awake()
        {
            if (!Instance) Instance = this;
        }
    }
}
