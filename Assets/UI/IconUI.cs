using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class IconUI : MonoBehaviour
    {
        public Image icon;
        
        private Tween iconAnimation = null;
        public bool isTweening => iconAnimation != null;
        
        private void OnEnable()
        {
            if (isTweening)
                iconAnimation.Kill();
            
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            iconAnimation = transform.DOScale(1, 0.3f);
        }

        private void OnDisable()
        {
            if (isTweening)
                iconAnimation.Kill();
        }
    }
}