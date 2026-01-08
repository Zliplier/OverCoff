using Players;
using Players.UI;
using UnityEngine;

namespace UI.Display
{
    public abstract class ObjectDisplay<T> where T : MonoBehaviour
    {
        //public List<Player> players { get; protected set; } = new();
        
        public T display = null;
        public bool isUIShown => display != null && display.gameObject.activeInHierarchy;
        
        protected bool useShowHide = false;

        public virtual void ShowUI(Player player)
        {
            if (isUIShown)
                return;
            
            //Debug.Log("Showing UI");
            
            if (!useShowHide)
                display = SpawnDisplay(player.playerUIManager);
            else
                display.gameObject.SetActive(true);
            
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

            //Debug.Log("Hiding UI");
            
            if (!useShowHide)
            {
                GameObject.Destroy(display.gameObject);
                display = null;
            }
            else
                display.gameObject.SetActive(false);
        }

        protected abstract T SpawnDisplay(PlayerUIManager playerUI);
    }
}