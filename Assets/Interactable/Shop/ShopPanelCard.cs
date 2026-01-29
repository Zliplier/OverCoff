using System;
using DG.Tweening;
using Inventory;
using Items.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Interactable.Shop
{
    public class ShopPanelCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private ShopPanelManager shopPanelManager;
        public InventoryManager shopInventory => shopPanelManager.shopInventory;
        public Image panelBg;
        public SO_Item soldItem;
        public Image itemIcon;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemPrice;
        
        [Header("Configs")]
        public Color deSelectedColor;
        public Color selectedColor;
        
        private Tween panelAnimation = null;
        public bool isTweening => panelAnimation != null;
        
        public void Initialize(ShopPanelManager shopPanelManager)
        {
            this.shopPanelManager = shopPanelManager;
            
            if (soldItem == null)
                return;
            itemIcon.sprite = soldItem.icon;
            itemName.SetText(soldItem.displayData.displayName);
            itemPrice.SetText(soldItem.cost.ToString());
        }
        
        public void Initialize(ShopPanelManager shopPanelManager, SO_Item soldItem)
        {
            this.shopPanelManager = shopPanelManager;
            
            this.soldItem = soldItem;
            itemIcon.sprite = soldItem.icon;
            itemName.SetText(soldItem.name);
            itemPrice.SetText(soldItem.cost.ToString());
        }
        
        private void OnEnable()
        {
            panelBg.color = deSelectedColor;
        }

        private void OnDisable()
        {
            panelBg.color = deSelectedColor;
            if (isTweening)
                panelAnimation.Kill();
            
            transform.localScale = Vector3.one;
        }
        
        private void Buy()
        {
            if (shopPanelManager.player.money < soldItem.cost)
                return;
            
            //If the pointer is not empty.
            if (shopInventory.pointerDisplayItem != null)
            {
                //Check if the sold Item is the same as the pointer item.
                if (string.Equals(shopInventory.pointerDisplayItem.InventoryItem.data.nameId, soldItem.nameID,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    //Add 1 Item to the stack
                    shopInventory.pointerDisplayItem.AddStack(1);
                }
                
            }
            //Its empty.
            else
            {
                //Add 1 Item to hand.
                InventoryItem item = new InventoryItem(soldItem, 1);
                item.Initialize();
                
                shopPanelManager.shopInventory.pointerDisplayItem = DisplayItem.CreateItemDisplay(item, shopInventory.root);
                shopPanelManager.shopInventory.pointerDisplayItem.StartDrag();
            }
            
            shopPanelManager.player.money -= soldItem.cost;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            panelBg.color = selectedColor;
            if (isTweening)
                panelAnimation?.Kill();
            panelAnimation = transform.DOScale(1.1f, 0.3f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            panelBg.color = deSelectedColor;
            if (isTweening)
                panelAnimation?.Kill();
            panelAnimation = transform.DOScale(1f, 0.3f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Buy();
            PlaySlotBounceAnimation(false);
        }
        
        public void PlaySlotBounceAnimation(bool scaleOutward = true, float duration = 0.3f)
        {
            if (isTweening)
                panelAnimation?.Kill();
            
            panelAnimation = transform.DOScale(scaleOutward ? 1.1f : 0.9f, duration / 2);
            
            panelAnimation.onComplete += () => 
                panelAnimation = transform.DOScale(1f, duration / 2);
        }
    }
}