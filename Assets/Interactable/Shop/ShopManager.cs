using System;
using DG.Tweening;
using Inventory;
using Items.Data;
using Players;
using UI;
using UnityEngine;

namespace Interactable.Shop
{
    [RequireComponent(typeof(Interactor))]
    public class ShopManager : MonoBehaviour
    {
        private Interactor interactor;
        
        public GameObject shopUIPrefab;
        
        public ShopPanelManager shopPanelManager { get; private set; }
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
            
            player.uiInputMap.cancelEvent += Close;
            player.uiInputMap.inventoryEvent += Close;
            
            shopPanelManager = Instantiate(shopUIPrefab, interactionUI.GetPanel("Shop").panelRoot.transform).GetComponent<ShopPanelManager>();
            shopPanelManager.name = "Shop UI";
            
            PlayPopUpAnimation(true).onComplete += () =>
            {
                player.SetCursorLockState(false);

                player.playerInputMap.SetMapEnable(false);
                player.uiInputMap.SetMapEnable(true);
                storageAnimation = null;
            };
            
            shopPanelManager.Initialize(this, player);
            shopPanelManager.AddShopPanel(soldItems);
        }

        public void Close(bool isHold)
        {
            player.uiManager.GetUISection("Main").sectionRoot.SetActive(true);
            
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
                
                Destroy(shopPanelManager.gameObject);
                player = null;
            };
        }
        
        private Tween PlayPopUpAnimation(bool isOpen)
        {
            if (isTweening)
                storageAnimation.Kill();

            if (isOpen)
            {
                shopPanelManager.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                storageAnimation = shopPanelManager.transform.DOScale(1f, 0.2f);
            }
            else
            {
                storageAnimation = shopPanelManager.transform.DOScale(0.8f, 0.1f);
            }
            
            return storageAnimation;
        }
    }
}