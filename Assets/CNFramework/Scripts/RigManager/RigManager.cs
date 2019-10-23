using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNFramework
{
    public class RigManager : MonoBehaviour
    {
        public static RigManager Instance;

        [SerializeField] private ControllerInputManager leftHandInput;
        public ControllerInputManager LeftHandInput => leftHandInput;

        [SerializeField] private ControllerInputManager rightHandInput;
        public ControllerInputManager RightHandInput => rightHandInput;

        private void Awake()
        {
            if (!Instance) Instance = this;
        }
    }
}
