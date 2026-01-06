using System;
using Items.Data;
using Players.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class Interactor : MonoBehaviour
    {
        public bool showInteractPrompt;
        
        [Header("Events")]
        public UnityEvent<GameObject> onInteract;
        public UnityEvent<GameObject> onHover, onUnHover;
        
        protected virtual void Start()
        {
            onHover.AddListener(ShowOutline);
            onUnHover.AddListener(HideOutline);
        }
        
        public virtual void Interact(GameObject user) => onInteract.Invoke(user);
        
        protected virtual void ShowOutline(GameObject user)
        {
            
        }

        protected virtual void HideOutline(GameObject user)
        {
            
        }
    }
}