using UnityEditor;
using UnityEngine;

namespace CNFramework.Utility
{
    [InitializeOnLoad]
    public class ProjectBindingInitialization
    {
        private static bool _isInitialized;
        
        static ProjectBindingInitialization()
        {
            if (_isInitialized) return;
            
            SetupTagBindings();
            SetupLayerBindings();
            SetupInputBindings();

            _isInitialized = true;
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
