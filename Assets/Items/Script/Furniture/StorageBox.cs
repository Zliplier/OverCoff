using System;
using DG.Tweening;
using Interactable;
using Inventory;
using Players;
using UI;
using UnityEngine;

namespace Items.Script.Furniture
{
    [RequireComponent(typeof(InventoryManager), typeof(Interactor))]
    public class StorageBox : ItemScript
    {
        public const string SECTION_NAME = "Storage";
        
        private Interactor interactor;
        
        public InventoryManager storageManager;
        public GameObject storageUIPrefab;
        
        public StorageUIWrapper storageUIWrapper { get; private set; }
        private Player player;

        private Tween storageAnimation = null;
        public new bool isTweening => storageAnimation != null;

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

            storageUIWrapper = Instantiate(storageUIPrefab, interactionUI.GetPanel("Storage").panelRoot.transform).GetComponent<StorageUIWrapper>();
            storageUIWrapper.name = "Storage UI";
            //storageUIWrapper.transform.SetParent(interactionUI.GetPanel("Storage").panelRoot.transform);
            //storageUIWrapper.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
            
            player.playerInventory.GetSection("Inventory").AddDisplay(storageUIWrapper.inventoryDisplay, storageManager);
            player.playerInventory.GetSection("Hand").AddDisplay(storageUIWrapper.handSlotDisplay, storageManager);
            storageManager.GetSection("Storage").AddDisplay(storageUIWrapper.storageDisplay, storageManager);
            
            PlayPopUpAnimation(true).onComplete += () =>
            {
                player.SetCursorLockState(false);

                player.playerInputMap.SetMapEnable(false);
                player.uiInputMap.SetMapEnable(true);
                storageAnimation = null;
            };
        }

        public void Close(bool isHold)
        {
            player.playerInventory.GetSection("Inventory").RemoveDisplay(storageUIWrapper.inventoryDisplay);
            player.playerInventory.GetSection("Hand").RemoveDisplay(storageUIWrapper.handSlotDisplay);
            storageManager.GetSection("Storage").RemoveDisplay(storageUIWrapper.storageDisplay);
            
            player.uiInputMap.cancelEvent -= Close;
            player.uiInputMap.inventoryEvent -= Close;
            
            PlayPopUpAnimation(false).onComplete += () =>
            {
                player.SetCursorLockState(true);

                player.playerInputMap.SetMapEnable(true);
                player.uiInputMap.SetMapEnable(false);
                
                storageAnimation = null;
                
                player.uiManager.GetUISection("Interaction").sectionRoot.SetActive(false);
                
                if (isTweening)
                    storageAnimation.Kill();
                
                Destroy(storageUIWrapper.gameObject);
                player = null;
            };
        }
        
        private Tween PlayPopUpAnimation(bool isOpen)
        {
            if (isTweening)
                storageAnimation.Kill();

            if (isOpen)
            {
                storageUIWrapper.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                storageAnimation = storageUIWrapper.transform.DOScale(1f, 0.2f);
            }
            else
            {
                storageAnimation = storageUIWrapper.transform.DOScale(0.8f, 0.1f);
            }
            
            return storageAnimation;
        }
    }
}