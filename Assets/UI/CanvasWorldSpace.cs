using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.WorldSpaceUI
{
    public class CanvasWorldSpace : MonoBehaviour
    {
        [SerializeField] private Image icon;
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ChangeIcon(Sprite newIcon)
        {
            icon.sprite = newIcon;
        }
        
        private void LateUpdate()
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }
}