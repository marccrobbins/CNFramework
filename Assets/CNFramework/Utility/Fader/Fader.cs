using System;
using System.Collections;
using UnityEngine;

namespace CNFramework.Utility
{
    public class Fader : MonoBehaviour
    {
        private static Fader _instance;

        [SerializeField] private Renderer fadeQuad;
        [SerializeField] private Material fadeMat;
        [SerializeField] private float blinkDuration;
        [SerializeField] private float fadeDuration;
        [SerializeField] private bool startClear;

        public FadeState _currentState;
        public static FadeState CurrentState => _instance._currentState;

        private static Material _instancedFadeMat;
        private static int _fadeId;
        
        private void Start()
        {
            if (!_instance)
            {
                _instance = this;
                _instancedFadeMat = Instantiate(fadeMat);
                fadeQuad.material = _instancedFadeMat;
            }

            if (startClear)
            {
                FadeIn();
            }
            else
            {
                FadeOut();
            }
        }

        [ContextMenu("Blink")]
        private void Blink()
        {
            Blink(null, null);
        }

        [ContextMenu("FadeIn")]
        private void FadeIn()
        {
            FadeIn(null);
        }
        
        [ContextMenu("FadeOut")]
        private void FadeOut()
        {
            FadeOut(null);
        }

        public static void Blink(Action onCompleteBlinkCallback = null, Action onCompleteCallback = null)
        {
            FadeOut(_instance.blinkDuration, () =>
            {
                onCompleteBlinkCallback?.Invoke();
                FadeIn(_instance.blinkDuration, onCompleteCallback);
            });
        }

        public static void FadeIn(Action onCompleteCallback = null)
        {
            FadeIn(_instance.fadeDuration);
        }

        public static void FadeIn(float duration, Action onCompleteCallback = null)
        {
            if (_instance._currentState == FadeState.Clear)
            {
                Debug.LogError("Cannot initiate FadeIn while already Clear.");
                return;
            }
            
            _instance.StartCoroutine(FadeRoutine(FadeState.Clear, duration, onCompleteCallback));
        }
        
        public static void FadeOut(Action onCompleteCallback = null)
        {
            FadeOut(_instance.fadeDuration);
        }

        public static void FadeOut(float duration, Action onCompleteCallback = null)
        {
            if (_instance._currentState == FadeState.Filled)
            {
                Debug.LogError("Cannot initiate FadeOut while already Filled.");
                return;
            }
            
            _instance.StartCoroutine(FadeRoutine(FadeState.Filled, duration, onCompleteCallback));
        }

        private static IEnumerator FadeRoutine(FadeState nextState, float duration, Action onCompleteCallback = null)
        {
            _instance._currentState = FadeState.Fading;

            var min = _instancedFadeMat.GetFloat("_fade");
            var max = nextState == FadeState.Clear ? 0 : 1;
            var timePassed = 0f;
            while (timePassed < duration)
            {
                timePassed += Time.deltaTime;
                var result = Mathf.MoveTowards(min, max, timePassed / duration);
                _instancedFadeMat.SetFloat("_fade", result);
                
                yield return new WaitForFixedUpdate();
            }
            
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.15f);

            _instance._currentState = nextState;
            onCompleteCallback?.Invoke();
        }
        
        public enum FadeState
        {
            Fading,
            Filled,
            Clear
        }
    }
}
