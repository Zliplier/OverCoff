using System;
using DG.Tweening;
using UnityEngine;

namespace Interactable.Object
{
    public class Shutter : MonoBehaviour
    {
        public Transform openPosition;
        public Transform closedPosition;
        public GameObject shutter;
        
        private Tween tw_Anim = null;
        public bool isAnimating => tw_Anim != null;

        public bool isOpened = true;
        public float duration = 1f;

        [Button(nameof(Open))] public bool OpenButton;
        [Button(nameof(Close))] public bool CloseButton;

        private void Start()
        {
            if (isOpened)
                shutter.transform.position = openPosition.position;
            else
            {
                shutter.transform.position = closedPosition.position;
            }
        }

        public void Open()
        {
            if (isOpened)
                return;
            if (isAnimating)
            {
                tw_Anim.Kill();
                tw_Anim = null;
            }
            
            isOpened = true;
            PlayAnim(true);
        }

        public void Close()
        {
            if (!isOpened)
                return;
            if (isAnimating)
            {
                tw_Anim.Kill();
                tw_Anim = null;
            }
            
            isOpened = false;
            PlayAnim(false);
        }

        private void PlayAnim(bool isOpen)
        {
            Vector3 endPoint = isOpen ? openPosition.localPosition : closedPosition.localPosition;

            tw_Anim = shutter.transform.DOMoveY(endPoint.y, duration);
            tw_Anim.SetEase(Ease.InOutQuad);
            tw_Anim.onComplete += () =>
            {
                tw_Anim = null;
            };
        }
    }
}