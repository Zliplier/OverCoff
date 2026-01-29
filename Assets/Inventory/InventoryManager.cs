using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Data;
using Items;
using Items.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public const string INVENTORY_ITEM_PREFAB_PATH = "Inventory/Item";
        
        public Transform root;
        
        [HideInInspector] public InventoryData data;
        
        public InventorySection[] inventorySections;
        [HideInInspector] public DisplayItem pointerDisplayItem;
        
        public UnityEvent onOpenInventory;
        public UnityEvent onCloseInventory;
        
        public void OnOpenInventory()
        {
            onOpenInventory?.Invoke();
        }
        
        public void OnCloseInventory()
        {
            if (pointerDisplayItem != null)
                Destroy(pointerDisplayItem.gameObject);
            
            /*if (pointerDisplayItem.parentAfterDrag.childCount == 0)
                pointerDisplayItem.EndDrag();
            else
                Destroy(pointerDisplayItem.gameObject);*/
            
            pointerDisplayItem = null;
            
            onCloseInventory?.Invoke();
        }

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            foreach (InventorySection section in inventorySections)
            {
                section.Initialize(this);
            }
        }

        public InventorySection GetSection(string sectionName) =>
            inventorySections.FirstOrDefault(s
                => string.Equals(sectionName, s.sectionName, StringComparison.InvariantCultureIgnoreCase));
        
    }
}