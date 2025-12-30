using System;
using System.Collections;
using Items.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zlipacket.CoreZlipacket.Player.Input;
using Zlipacket.CoreZlipacket.Tools;

namespace Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        public bool useSO_Item;
        [DrawIf("useSO_Item")] public SO_Item so_Item;
        
        [HideInInspector] public ItemData item;
        public int stack => item.stack;
        public int maxStack => item.maxStack;
        
        [Header("UI")]
        public Image image;
        
        [HideInInspector] public Transform parentAfterDrag;
        
        private Coroutine co_Drag = null;
        public bool isDragging => co_Drag != null;
        
        public void Initialize(ItemData newItem)
        {
            if (newItem == null && useSO_Item)
                item = new ItemData(so_Item.itemData);
            else
                item = new ItemData(newItem);
            
            image.sprite = item.icon;
        }
        
        public Coroutine StartDrag()
        {
            //Debug.Log("StartDrag");
            if (isDragging)
                return co_Drag;
            
            parentAfterDrag = transform.parent;
            
            co_Drag = StartCoroutine(Dragging());
            return co_Drag;
        }
        
        public Coroutine EndDrag()
        {
            //Debug.Log("EndDrag");
            if (!isDragging)
                return co_Drag;
            
            StopCoroutine(co_Drag);
            
            transform.SetParent(parentAfterDrag);
            
            co_Drag = null;
            return co_Drag;
        }

        private IEnumerator Dragging()
        {
            while (true)
            {
                //Debug.Log("Dragging");
                transform.position = Input.mousePosition;
                
                yield return null;
            }
        }
    }
}