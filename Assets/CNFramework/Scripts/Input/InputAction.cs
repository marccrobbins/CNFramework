using UnityEngine;

namespace CNFramework
{
    public class InputAction : MonoBehaviour
    {
        [SerializeField] private Process process;

        private void FixedUpdate()
        {
            if (process == Process.FixedUpdate)
            {
                ProcessAction();
            }
        }

        private void Update()
        {
            if (process == Process.Update)
            {
                ProcessAction();
            }
        }

        private void LateUpdate()
        {
            if (process == Process.LateUpdate)
            {
                ProcessAction();
            }
        }

        private void OnPreRender()
        {
            if (process == Process.PreRender)
            {
                ProcessAction();
            }
        }

        private void OnPostRender()
        {
            if (process == Process.PostRender)
            {
                ProcessAction();
            }
        }

        private void OnPreCull()
        {
            if (process == Process.PreCull)
            {
                ProcessAction();
            }
        }

        protected virtual void ProcessAction() { }
    }

    public enum Process
    {
        Update = 0,
        LateUpdate,
        FixedUpdate,
        PreRender,
        PostRender,
        PreCull
    }
}