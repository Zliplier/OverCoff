using DG.Tweening;
using Players.UI;
using UnityEngine;

namespace Players.PlayerScripts
{
    public class PlayerBook : PlayerScript
    {
        [SerializeField] private GameObject pageRoot;
        
        public UISection bookSection => playerUIManager.GetUISection("Book");
        
        public bool isBookOpen => bookSection.sectionRoot.activeInHierarchy;
        
        public BookPage currentPage;
        
        private Tween bookAnimation = null;
        public bool isTweening => bookAnimation != null;
        
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

            PlayPopUpAnimation(true).onComplete += () =>
            {
                playerInputMap.SetMapEnable(false);
                uiInputMap.SetMapEnable(true);
            };
        }

        public void CloseBook(bool isHolding)
        {
            if (!isBookOpen)
                return;
            
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

            PlayPopUpAnimation(false).onComplete += () =>
            {
                player.SetCursorLockState(true);

                playerInputMap.SetMapEnable(true);
                uiInputMap.SetMapEnable(false);

                bookSection.sectionRoot.SetActive(false);
            };
        }

        private Tween PlayPopUpAnimation(bool isOpen)
        {
            if (isTweening)
                bookAnimation.Kill();

            if (isOpen)
            {
                pageRoot.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                bookAnimation = pageRoot.transform.DOScale(1f, 0.2f);
            }
            else
            {
                bookAnimation = pageRoot.transform.DOScale(0.8f, 0.1f);
            }
            
            return bookAnimation;
        }
        
        private void OpenInventory()
        {
            playerInventory.OnOpenInventory();
        }

        private void CloseInventory() => playerInventory.OnCloseInventory();
    }

    public enum BookPage
    {
        Inventory, 
        Quest, 
        Recipe
    }
}