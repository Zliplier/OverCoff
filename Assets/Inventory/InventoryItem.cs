using System;
using System.Collections;
using Items.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zlipacket.CoreZlipacket.Player.Input;
using Zlipacket.CoreZlipacket.Tools;

namespace Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        public InventorySlot slot { get; private set; }
        
        public bool useSO_Item;
        [DrawIf("useSO_Item")] public SO_Item so_Item;
        
        public ItemData data;

        public int stack
        {
            get
            { return data.stack; }
            set
            {
                data.stack = Math.Clamp(value, 0, maxStack); 
                onStackChanged.Invoke(value);
            }
        }
        public int maxStack => data.maxStack;
        
        [Header("UI")]
        public Image iconImage;
        public TextMeshProUGUI stackText;
        
        [Header("Events")]
        public UnityEvent<int> onStackChanged;
        
        [HideInInspector] public Transform parentAfterDrag;
        
        private Coroutine co_Drag = null;
        public bool isDragging => co_Drag != null;
        
        public void Initialize(ItemData newItem, InventorySlot slot)
        {
            if (newItem == null || useSO_Item)
                data = new ItemData(so_Item.itemData);
            else
                data = new ItemData(newItem);
            
            this.slot = slot;
            
            onStackChanged.AddListener(SetStackText);
            
            iconImage.sprite = data.icon;
            SetStackText(stack);
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

        public void AddStack(int amount)
        {
            stack += amount;
        }

        public void RemoveStack(int amount)
        {
            stack -= amount;
            
            if (stack <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void SetStackText(int stack)
        {
            if (stack <= 1)
                stackText.enabled = false;
            else
                stackText.enabled = true;
            
            stackText.text = stack.ToString();
        }
        
        public static bool TryStackItem(ItemData fromItem, ItemData toItem)
        {
            //Check for the same name Id.
            if (string.Equals(fromItem.nameId, toItem.nameId,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                //Check if stack is stackable and not full.
                if (toItem.maxStack > 1 && toItem.stack < toItem.maxStack)
                    return true;
            }
            
            return false;
        }
        
        public static void StackItemToStack(InventoryItem fromStack, InventoryItem toStack)
        {
            int toStackCapacity = toStack.maxStack - toStack.stack;
            int transferStack;
            
            //Check if fromStack is less than or equal to toStack capacity so we can move the entire fromStack.
            if (fromStack.stack <= toStackCapacity)
                transferStack = fromStack.stack;
            //fromStack is greater than toStack so after the transfer the fromStack will have some left.
            else
                transferStack = toStackCapacity;
            
            toStack.AddStack(transferStack);
            fromStack.RemoveStack(transferStack);
        }
        
        public static void StackItemToStack(InventoryItem fromStack, InventoryItem toStack, int amount)
        {
            int toStackCapacity = toStack.maxStack - toStack.stack;
            int transferStack;
            
            //Check if he amount to transfer is greater than the capacity toStack can hold.
            if (amount > toStackCapacity)
                amount = toStackCapacity;
            
            //fromStack is not enough to the amount so we transfer all of fromStack as much as we can.
            if (amount >= fromStack.stack)
                transferStack = fromStack.stack;
            //fromStack is enough so we transfer the amount.
            else
                transferStack = amount;
            
            toStack.AddStack(transferStack);
            fromStack.RemoveStack(transferStack);
        }
    }
}