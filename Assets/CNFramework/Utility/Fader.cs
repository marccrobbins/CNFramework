using System;
using UnityEngine;

namespace CNFramework.Utility
{
    public class Fader : MonoBehaviour
    {
        private static Fader _instance;

        [SerializeField] private Material _fadeMat;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private bool _startClear;

        private FadeState _currentState;
        public static FadeState CurrentState => _instance._currentState;

        private float _fadeValue;
        private FadeState _nextState;
        private FadeState _prevState;
        private bool _canFade;
        private float _fadeToValue;
        private Action _onComplete;

        void Start()
        {
            if (!_instance) _instance = this;
            else return;

            if (_startClear)
            {
                _currentState = FadeState.Clear;
                _fadeValue = 1;
            }
            else
            {
                _currentState = FadeState.Filled;
                _fadeValue = 0;
            }
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (_canFade)
            {
                float time = Time.fixedDeltaTime * _fadeDuration;
                _fadeValue = Mathf.MoveTowards(_fadeValue, _fadeToValue, time);

                if (_fadeValue.Equals(_fadeToValue))
                {
                    switch (_prevState)
                    {
                        case FadeState.Clear:
                            _currentState = FadeState.Filled;
                            break;
                        case FadeState.Filled:
                            _currentState = FadeState.Clear;
                            break;
                    }

                    _canFade = false;
                    _onComplete?.Invoke();
                    _onComplete = null;
                }
            }

            Material mat = Instantiate(_fadeMat);

            Color updatedColor = mat.color;
            updatedColor.r = _fadeValue;
            updatedColor.g = _fadeValue;
            updatedColor.b = _fadeValue;
            mat.color = updatedColor;

            Graphics.Blit(src, dest, mat);
        }

        public static void FadeOut(Action onComplete = null)
        {
            _instance._onComplete = onComplete;
            _instance.Fade(FadeState.Filled);
        }

        public static void FadeIn(Action onComplete = null)
        {
            _instance._onComplete = onComplete;
            _instance.Fade(FadeState.Clear);
        }

        private void Fade(FadeState nextSate)
        {
            _nextState = nextSate;

            if (_fadeDuration <= 0)
            {
                Debug.Log("Cannot fade if speed of fade is zero");
                return;
            }

            switch (_currentState)
            {
                case FadeState.Fading:
                    Debug.Log("Cannot call fade while already fading.");
                    _onComplete = null;
                    return;
                case FadeState.Clear:
                case FadeState.Filled:
                    if (_nextState == _currentState)
                    {
                        Debug.Log("Cannot fade to same state of FadeEffect");
                        return;
                    }

                    break;
            }

            _prevState = _currentState;

            switch (_nextState)
            {
                case FadeState.Clear:
                    _fadeToValue = 1;
                    break;
                case FadeState.Filled:
                    _fadeToValue = 0;
                    break;
            }

            _canFade = true;
            _currentState = FadeState.Fading;
        }

        public enum FadeState
        {
            Fading,
            Filled,
            Clear
        }
    }
}
