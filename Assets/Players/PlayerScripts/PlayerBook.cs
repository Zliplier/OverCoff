using Players.UI;
using UnityEngine;

namespace Players.PlayerScripts
{
    public class PlayerBook : PlayerScript
    {
        public UISection bookSection => playerUIManager.GetUISection("Book");
        
        public bool isBookOpen => bookSection.sectionRoot.activeInHierarchy;
        
        public BookPage currentPage;
        
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
            switch (page)
            {
                case BookPage.Inventory:
                    OpenInventory();
                    break;
                case BookPage.Quest:
                    
                    break;
                case BookPage.Recipe:
                    
                    break;
            }
        }

        public void CloseBook(bool isHolding)
        {
            if (!isBookOpen)
                return;
            
            player.SetCursorLockState(true);
            
            playerInputMap.SetMapEnable(true);
            uiInputMap.SetMapEnable(false);
            
            switch (currentPage)
            {
                case BookPage.Inventory:
                    CloseInventory();
                    break;
                case BookPage.Quest:
                    
                    break;
                case BookPage.Recipe:
                    
                    break;
            }
            
            bookSection.sectionRoot.SetActive(false);

            
        }

        private void OpenInventory() => playerInventory.OnOpenInventory();
        private void CloseInventory() => playerInventory.OnCloseInventory();
    }

    public enum BookPage
    {
        Inventory, 
        Quest, 
        Recipe
    }
}