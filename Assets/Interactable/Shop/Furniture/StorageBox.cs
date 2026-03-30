using DG.Tweening;
using Inventory;
using Items;
using Players;
using UI;
using UnityEngine;

namespace Interactable.Shop.Furniture
{
    [RequireComponent(typeof(InventoryManager), typeof(Interactor))]
    public class StorageBox : ItemScript
    {
        public const string SECTION_NAME = "Storage";
        
        private Interactor interactor;
        
        public InventoryManager storageManager;
        public GameObject storageUIPrefab;
        
        private Player player;

        private Tween storageAnimation = null;
        public new bool isTweening => storageAnimation != null;

        private void OnDisable()
        {
            if (isTweening)
                storageAnimation?.Kill();
        }
        
        private void Start()
        {
            interactor = GetComponent<Interactor>();
            interactor.onInteract.AddListener(Open);
        }

        public void Open(Player player)
        {
            this.player = player;
            
            UISection interactionUI = player.uiManager.GetUISection("Interaction");
            interactionUI.sectionRoot.SetActive(true);
            
            storageManager.root = interactionUI.sectionRoot.transform;
            
            player.uiInputMap.cancelEvent += Close;
            player.uiInputMap.inventoryEvent += Close;

            /*storageDisplayWrapper = Instantiate(storageUIPrefab, interactionUI.GetPanel("Storage").panelRoot.transform).GetComponent<StorageDisplayWrapper>();
            storageDisplayWrapper.name = "Storage UI";
            //storageUIWrapper.transform.SetParent(interactionUI.GetPanel("Storage").panelRoot.transform);
            //storageUIWrapper.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
            
            player.playerInventory.GetSection("Inventory").AddDisplay(storageDisplayWrapper.playerInventoryDisplayWrapper.inventoryDisplay, storageManager);
            storageManager.GetSection("Storage").AddDisplay(storageDisplayWrapper.storageDisplay, storageManager);*/
            
            /*PlayPopUpAnimation(true).onComplete += () =>
            {
                player.SetCursorLockState(false);

                player.playerInputMap.SetMapEnable(false);
                player.uiInputMap.SetMapEnable(true);
                storageAnimation = null;
            };*/
        }

        public void Close(bool isHold)
        {
            /*player.playerInventory.GetSection("Inventory").RemoveDisplay(storageDisplayWrapper.playerInventoryDisplayWrapper.inventoryDisplay);
            player.playerInventory.GetSection("Hand").RemoveDisplay(storageDisplayWrapper.playerInventoryDisplayWrapper.handSlotDisplay);
            storageManager.GetSection("Storage").RemoveDisplay(storageDisplayWrapper.storageDisplay);*/
            
            player.uiInputMap.cancelEvent -= Close;
            player.uiInputMap.inventoryEvent -= Close;
            
            /*PlayPopUpAnimation(false).onComplete += () =>
            {
                player.SetCursorLockState(true);

                player.playerInputMap.SetMapEnable(true);
                player.uiInputMap.SetMapEnable(false);
                
                storageAnimation = null;
                
                player.uiManager.GetUISection("Interaction").sectionRoot.SetActive(false);
                
                if (isTweening)
                    storageAnimation.Kill();
                
                Destroy(storageDisplayWrapper.gameObject);
                player = null;
            };*/
        }
        
        /*private Tween PlayPopUpAnimation(bool isOpen)
        {
            if (isTweening)
                storageAnimation.Kill();

            if (isOpen)
            {
                storageDisplayWrapper.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                storageAnimation = storageDisplayWrapper.transform.DOScale(1f, 0.2f);
            }
            else
            {
                storageAnimation = storageDisplayWrapper.transform.DOScale(0.8f, 0.1f);
            }
            
            return storageAnimation;
        }*/
    }
}