using System;
using Items.ItemData;
using QuickOutline;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private Outline outline;
        [SerializeField] private SO_OutlineConfig outlineConfig;
        
        public UnityEvent onInteract;
        public UnityEvent onHover, onUnHover;
        
        private void Start()
        {
            outline.OutlineMode = outlineConfig.Mode;
            outline.OutlineColor = outlineConfig.Color;
            outline.OutlineWidth = outlineConfig.Width;
            outline.enabled = false;
        }
        
        public virtual void Interact() => onInteract.Invoke();
    }
}