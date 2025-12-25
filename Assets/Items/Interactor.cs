using System;
using Items.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class Interactor : MonoBehaviour
    {
        public UnityEvent onInteract;
        public UnityEvent onHover, onUnHover;
        
        protected virtual void Start()
        {
            onHover.AddListener(ShowOutline);
            onUnHover.AddListener(HideOutline);
        }
        
        public virtual void Interact() => onInteract.Invoke();
        
        protected virtual void ShowOutline()
        {
            
        }

        protected virtual void HideOutline()
        {
            
        }
    }
}