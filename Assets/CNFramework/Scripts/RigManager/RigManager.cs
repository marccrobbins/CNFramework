using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNFramework
{
    public class RigManager : MonoBehaviour
    {
        public static RigManager Instance;

        private void Awake()
        {
            if (!Instance) Instance = this;
        }
    }
}
