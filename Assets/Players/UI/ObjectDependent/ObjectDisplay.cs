using System.Collections.Generic;
using UnityEngine;

namespace Players.UI
{
    public abstract class ObjectDisplay<T> where T : MonoBehaviour
    {
        //public List<Player> players { get; protected set; } = new();
        
        public T display = null;
        public bool isUIShown => display != null;

        public virtual void ShowUI(Player player)
        {
            if (isUIShown)
                return;
            
            SpawnDisplay(player.GetComponent<PlayerUIManager>());
            
            /*if (!players.Contains(player))
                players.Add(player);*/
        }
        
        public virtual void HideUI(Player player)
        {
            HideUI();
        }
        
        public virtual void HideUI()
        {
            if (!isUIShown)
                return;
            
            GameObject.Destroy(display.gameObject);
            display = null;
        }

        protected abstract T SpawnDisplay(PlayerUIManager playerUI);
    }
}