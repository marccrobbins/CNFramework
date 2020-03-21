using UnityEditor;
using UnityEngine;

namespace CNFramework.Utility
{
    [InitializeOnLoad]
    public class ProjectBindingInitialization
    {
        static ProjectBindingInitialization()
        {
            SetupTagBindings();
            SetupLayerBindings();
            SetupInputBindings();
        }

        private static void SetupTagBindings()
        {
            Debug.Log("Setting up tag bindings");
        }

        private static void SetupLayerBindings()
        {
            Debug.Log("Setting up layer bindings");
        }

        private static void SetupInputBindings()
        {
            Debug.Log("Setting up input bindings");
        }
    }
}
