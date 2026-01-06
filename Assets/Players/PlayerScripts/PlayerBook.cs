using Players.UI;
using UnityEngine;

namespace Players.PlayerScripts
{
    public class PlayerBook : PlayerScript
    {
        public UISection bookSection => playerUIManager.GetUISection("Book");
        
        public bool isBookOpen => bookSection.sectionRoot.activeInHierarchy;

        private void OnEnable()
        {
            uiInputMap.cancelEvent += CloseBook;
            uiInputMap.inventoryEvent += CloseBook;
        }

        private void OnDisable()
        {
            uiInputMap.cancelEvent -= CloseBook;
            uiInputMap.inventoryEvent -= CloseBook;
        }
        
        public void OpenBook(BookPage page)
        {
            player.SetCursorLockState(false);
            
            playerInputMap.SetMapEnable(false);
            uiInputMap.SetMapEnable(true);
            
            bookSection.sectionRoot.SetActive(true);
            
            
            
            //TODO: Make more pages.
        }

        public void CloseBook(bool isHolding)
        {
            if (!isBookOpen)
                return;
            
            player.SetCursorLockState(true);
            
            playerInputMap.SetMapEnable(true);
            uiInputMap.SetMapEnable(false);
            
            bookSection.sectionRoot.SetActive(false);
        }
    }

    public enum BookPage
    {
        Inventory, 
        
        
    }
}