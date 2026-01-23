using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    public class DisplaySection : MonoBehaviour
    {
        private InventorySection section;
        
        public DisplaySlot[] Slots;
        
        public UnityEvent<DisplaySlot> onSectionDisplayChange;

        public void Initialize(InventorySection section, InventoryManager manager)
        {
            this.section = section;
            
            if (Slots.Length != section.sectionInventory.Length)
                Debug.Log($"Inventory of {name} does not have equal slots to {section.sectionName}.");

            for (int i = 0; i < section.sectionInventory.Length; i++)
            {
                //Check if target inventory is bigger than this inventory
                if (i >= Slots.Length)
                    break;
                
                Slots[i].Initialize(section, manager, i);
                Slots[i].onSlotChange.AddListener(arg0 =>
                {
                    onSectionDisplayChange?.Invoke(arg0);
                });
            }
        }
    }
}