using System;
using System.Collections.Generic;
using Inventory;
using Items.Data;
using Players;
using UI.Display;
using UnityEngine;

namespace Interactable.Shop
{
    public class ShopPanelManager : MonoBehaviour
    {
        [HideInInspector] public Player player;
        
        public Transform panelRoot;
        private ShopManager shopManager;

        public ValueDisplay moneyDisplay;
        public List<ShopPanelCard> panelCards;
        public GameObject panelPrefab;

        private void OnDisable()
        {
            player.onMoneyChanged.RemoveListener(UpdateMoney);
        }

        public void Initialize(ShopManager shopManager, Player player)
        {
            this.shopManager = shopManager;
            this.player = player;
            
            foreach (var shopPanel in panelCards)
            {
                shopPanel.Initialize(this);
            }
            
            player.onMoneyChanged.AddListener(UpdateMoney);
            UpdateMoney(player.money);
        }
        
        public void UpdateMoney(int amount) => moneyDisplay.UpdateValue(player.money.ToString());
        
        public void AddShopPanel(SO_Item[] items)
        {
            foreach (SO_Item item in items)
                AddShopPanel(item);
        }
        
        public void AddShopPanel(SO_Item item)
        {
            ShopPanelCard panel = Instantiate(panelPrefab, panelRoot).GetComponent<ShopPanelCard>();
            panel.Initialize(this, item);
            
            panelCards.Add(panel);
        }
    }
}