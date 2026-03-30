using System;
using DG.Tweening;
using Inventory;
using Items;
using Items.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Environment = Zlipacket.CoreZlipacket.Misc.Environment;

namespace Interactable.Shop
{
    public class ShopPanelCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private ShopPanelManager shopPanelManager;
        public Image panelBg;
        public SO_Item soldItem;
        public Image itemIcon;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemPrice;
        
        [Header("Configs")]
        public Color deSelectedColor;
        public Color selectedColor;
        
        [Header("Events")]
        public UnityEvent onClick;
        
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
            onClick?.Invoke();
            
            if (shopPanelManager.player.money < soldItem.cost)
                return;
            
            //Buying and Spawning Item
            Item newItem = Instantiate(soldItem.itemPrefab, ShopTrapdoor.Instance.spawnPoint.position, Quaternion.identity).GetComponent<Item>();
            newItem.transform.SetParent(Environment.Instance.root);
            newItem.Initialize(soldItem.itemData);
            newItem.itemData.stack = 1;
            
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