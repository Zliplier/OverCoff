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
        
        protected virtual void Start()
        {
            if (outline  == null)
                return;
            
            outline.OutlineMode = outlineConfig.Mode;
            outline.OutlineColor = outlineConfig.Color;
            outline.OutlineWidth = outlineConfig.Width;
            outline.enabled = false;
            
            onHover.AddListener(ShowOutline);
            onUnHover.AddListener(HideOutline);
        }
        
        public virtual void Interact() => onInteract.Invoke();
        
        protected virtual void ShowOutline()
        {
            outline.enabled = true;
        }

        protected virtual void HideOutline()
        {
            outline.enabled = false;
        }
    }
}