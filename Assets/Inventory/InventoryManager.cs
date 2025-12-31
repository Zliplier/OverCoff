using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Data;
using Items;
using Items.Data;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public const string INVENTORY_ITEM_PREFAB_PATH = "Inventory/Item";
        
        public Transform root;
        
        [HideInInspector] public InventoryData data;
        
        public InventorySlot[] inventorySlots;
        [HideInInspector] public InventoryItem pointerItem;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            if (pointerItem == null)
                return;
            
            if (pointerItem.parentAfterDrag.childCount == 0)
                pointerItem.EndDrag();
            else
                Destroy(pointerItem.gameObject);
            
            pointerItem = null;
        }

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            foreach (InventorySlot slot in inventorySlots)
            {
                slot.Initialize(this);
            }
        }

        public void AddItem(ItemData item)
        {
            InventorySlot emptySlot = inventorySlots.FirstOrDefault(x => x.isEmpty);
            if (emptySlot == null)
                return;
            
            emptySlot.AddItem(item);
        }

        public void AddItem(ItemData item, InventorySlot slot)
        {
            if (!slot.isEmpty)
                return;
            
            slot.AddItem(item);
        }

        public void SyncInventory(InventoryManager manager)
        {
            
            
            
        }
    }
}