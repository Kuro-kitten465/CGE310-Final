using System;
using System.Collections;
using UnityEngine;

namespace Kuro.GameSystem
{
    public class FadingHandler : MonoBehaviour
    {
        public enum FadingType
        {
            FadeIn, FadeOut
        }

        private Canvas _fadeCanvas;
        private CanvasGroup _canvasGroup;
        private bool _isFading = false;

        public Action OnFadeCompleted;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void StartFade(float fadeDuration, FadingType fadingType)
        {
            if (_isFading || _canvasGroup == null) return;

            float startAlpha = fadingType == FadingType.FadeIn ? 0f : 1f;
            float endAlpha = fadingType == FadingType.FadeIn ? 1f : 0f;

            _isFading = true;
            StartCoroutine(Fading(fadeDuration, startAlpha, endAlpha));
        }

        private IEnumerator Fading(float fadeDuration, float startAlpha, float endAlpha)
        {
            float elapsedTime = 0f;
            _canvasGroup.alpha = startAlpha;

            // Optional: block input during fade
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = false;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeDuration);
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                yield return null;
            }

            _canvasGroup.alpha = endAlpha;

            // Optional: allow input again if fully transparent
            if (endAlpha == 0f)
            {
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.interactable = false;
            }

            _isFading = false;
            OnFadeCompleted?.Invoke();
        }
    }
}
