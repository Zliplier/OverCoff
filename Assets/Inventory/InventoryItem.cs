using System;
using Items.Data;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class InventoryItem
    {
        public SO_Item item; 
        public int amount = 0;
        
        public int stack
        {
            get => data.stack;
            set
            {
                data.stack = Math.Clamp(value, 0, maxStack);
                amount = stack;
            }
        }
        public int maxStack => data.maxStack;
        
        [HideInInspector] public ItemData data;

        public void Initialize()
        {
            if (item == null)
                return;
            
            data = new ItemData(item.itemData);
            stack = amount;
        }
        
        public InventoryItem(SO_Item item, int amount = 1)
        {
            if (amount <= 0)
            {
                Debug.LogError($"Amount of inventory item can't be less or equal to 0.");
                return;
            }
            
            this.item = item;
            data = new ItemData(item.itemData);
            stack = amount;
        }
        
        public InventoryItem(ItemData data)
        {
            this.item = data.scriptableObject;
            this.data = new ItemData(data);
            if (stack <= 0)
                stack = 1;
            
            amount = stack;
        }
        
        public InventoryItem Copy()
        {
            InventoryItem copy = new InventoryItem(item, stack);
            return copy;
        }

        public void Clear()
        {
            item = null;
            data = null;
            amount = 0;
        }
        
        public void AddStack(int amount)
        {
            stack += amount;
        }

        public void RemoveStack(int amount)
        {
            stack -= amount;
        }
    }
}