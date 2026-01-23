using System;
using System.Collections.Generic;
using System.Linq;
using Items.Data;
using UnityEngine;
using Zlipacket.CoreZlipacket.Tools;

namespace Inventory
{
    [Serializable]
    public class InventorySection
    {
        public List<DisplaySection> displays;
        
        public InventoryManager manager { get; private set; }
        
        public string sectionName;
        public InventoryItem[] sectionInventory;
        
        public void Initialize(InventoryManager manager)
        {
            this.manager = manager;

            foreach (var item in sectionInventory)
            {
                item.Initialize();
            }
            
            foreach (DisplaySection display in displays)
            {
                display.Initialize(this, manager);
                display.onSectionDisplayChange.AddListener(SyncDisplay);
            }
        }

        public void AddDisplay(DisplaySection display, InventoryManager manager)
        {
            display.Initialize(this, manager);
            display.onSectionDisplayChange.AddListener(SyncDisplay);
            displays.Add(display);
        }

        public void RemoveDisplay(DisplaySection display)
        {
            display.onSectionDisplayChange.RemoveListener(SyncDisplay);
            displays.Remove(display);
        }

        public void SyncDisplay(DisplaySlot slot)
        {
            foreach (DisplaySection display in displays)
            {
                display.Slots[slot.slotIndex].SyncSlot();
            }
        }
    }
}