using System;
using DG.Tweening;
using Inventory;
using Items.Data;
using Items.Script.Furniture;
using Players;
using UI;
using UnityEngine;

namespace Interactable.Shop
{
    [RequireComponent(typeof(Interactor), typeof(InventoryManager))]
    public class ShopManager : MonoBehaviour
    {
        public InventoryManager shopInventory;
        private Interactor interactor;
        
        public GameObject shopUIPrefab;
        
        public PlayerInventoryDisplayWrapper playerInventoryDisplayWrapper { get; private set; }
        private Player player;
        
        private Tween storageAnimation = null;
        public bool isTweening => storageAnimation != null;
        
        public SO_Item[] soldItems;

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
            player.uiManager.GetUISection("Main").sectionRoot.SetActive(false);
            
            shopInventory.root = interactionUI.sectionRoot.transform;
            
            player.uiInputMap.cancelEvent += Close;
            player.uiInputMap.inventoryEvent += Close;
            
            playerInventoryDisplayWrapper = Instantiate(shopUIPrefab, interactionUI.GetPanel("Shop").panelRoot.transform).GetComponent<PlayerInventoryDisplayWrapper>();
            playerInventoryDisplayWrapper.name = "Shop UI";
            
            player.playerInventory.GetSection("Inventory").AddDisplay(playerInventoryDisplayWrapper.inventoryDisplay, shopInventory);
            player.playerInventory.GetSection("Hand").AddDisplay(playerInventoryDisplayWrapper.handSlotDisplay, shopInventory);
            
            PlayPopUpAnimation(true).onComplete += () =>
            {
                player.SetCursorLockState(false);

                player.playerInputMap.SetMapEnable(false);
                player.uiInputMap.SetMapEnable(true);
                storageAnimation = null;
            };
            
            ShopPanelManager panelManager = playerInventoryDisplayWrapper.GetComponent<ShopPanelManager>();
            panelManager.Initialize(this, player);
            panelManager.AddShopPanel(soldItems);
        }

        public void Close(bool isHold)
        {
            player.uiManager.GetUISection("Main").sectionRoot.SetActive(true);
            
            player.playerInventory.GetSection("Inventory").RemoveDisplay(playerInventoryDisplayWrapper.inventoryDisplay);
            player.playerInventory.GetSection("Hand").RemoveDisplay(playerInventoryDisplayWrapper.handSlotDisplay);
            
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
                
                Destroy(playerInventoryDisplayWrapper.gameObject);
                player = null;
            };
        }
        
        private Tween PlayPopUpAnimation(bool isOpen)
        {
            if (isTweening)
                storageAnimation.Kill();

            if (isOpen)
            {
                playerInventoryDisplayWrapper.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                storageAnimation = playerInventoryDisplayWrapper.transform.DOScale(1f, 0.2f);
            }
            else
            {
                storageAnimation = playerInventoryDisplayWrapper.transform.DOScale(0.8f, 0.1f);
            }
            
            return storageAnimation;
        }
    }
}