using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Display
{
    public class ValueDisplay : MonoBehaviour
    {
        public TextMeshProUGUI textDisplay;

        private Tween textAnimation = null;
        public bool isTweening => textAnimation != null;
        
        private void OnDisable()
        {
            textAnimation?.Kill();
            textAnimation = null;
        }

        public void UpdateValue(string text)
        {
            textDisplay.SetText(text);
            PlayBounceAnimation(textDisplay.transform);
        }
        
        private void PlayBounceAnimation(Transform target, float duration = 0.3f)
        {
            textAnimation = target.DOScale(1.1f, duration / 2);
            textAnimation.onComplete += () =>
            {
                target.DOScale(1f, duration / 2);
                textAnimation = null;
            };
        }
    }
}