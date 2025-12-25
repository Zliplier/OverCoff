using System;
using System.Collections.Generic;
using Items;
using Items.Data;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "Inventory Object", menuName = "Inventory/Inventory Object")]
    public class SO_InventoryObject : ScriptableObject
    {
        public List<InventorySlot> inventorySlots = new List<InventorySlot>();

        public void AddItem(ItemData item, int stack = 1)
        {
            
        }
    }

    [Serializable]
    public class Inventory
    {
        public List<InventorySlot> inventory = new List<InventorySlot>();
        
        
    }
    
    [Serializable]
    public class InventorySlot
    {
        public ItemData item;
        public int stack => item.stack;
        public int maxStack => item.maxStack;

        public InventorySlot(ItemData item, int stack = 1)
        {
            this.item = item;
            this.item.stack = stack;
        }

        public void AddValue(int amount)
        {
            item.stack = Math.Clamp(item.stack + amount, 0, item.maxStack);
        }
    }
}