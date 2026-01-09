using Players;
using UnityEngine;
using UnityEngine.Events;

namespace Interactable
{
    public class Interactor : MonoBehaviour
    {
        public bool showInteractPrompt;
        
        [Header("Events")]
        public UnityEvent<Player> onInteract;
        public UnityEvent<Player> onHover, onUnHover, onHovering;
        
        protected virtual void Start()
        {
            onHover.AddListener(ShowOutline);
            onUnHover.AddListener(HideOutline);
        }
        
        public virtual void Interact(Player user) => onInteract.Invoke(user);
        
        protected virtual void ShowOutline(Player user)
        {
            
        }

        protected virtual void HideOutline(Player user)
        {
            
        }
    }
}