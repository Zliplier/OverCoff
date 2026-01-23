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
    public class DisplayItem : MonoBehaviour
    {
        [HideInInspector] public DisplaySlot DisplaySlot;
        [HideInInspector] public InventoryItem InventoryItem;
        public ItemData data => InventoryItem.data;

        [Header("UI")] public Image iconImage;
        public TextMeshProUGUI stackText;

        [HideInInspector] public Transform parentAfterDrag;

        private Coroutine co_Drag = null;
        public bool isDragging => co_Drag != null;

        public void Initialize(ref InventoryItem newItem, DisplaySlot displaySlot)
        {
            InventoryItem = newItem;
            DisplaySlot = displaySlot;

            iconImage.sprite = data.icon;
            SetStackText(data.stack);
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
            transform.localScale = Vector3.one;

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

        public void SyncItem()
        {
            iconImage.sprite = data.icon;
            SetStackText(data.stack);
        }

        public void AddStack(int amount)
        {
            InventoryItem.AddStack(amount);
            SyncItem();
            DisplaySlot?.onSlotChange?.Invoke(DisplaySlot);
        }

        public void RemoveStack(int amount)
        {
            if (InventoryItem.stack - amount <= 0)
            {
                if (DisplaySlot != null)
                    DisplaySlot?.ClearSlot();
                //Its pointer Item.
                else
                {
                    Destroy(gameObject);
                }
                return;
            }
            
            InventoryItem.RemoveStack(amount);
            SyncItem();
            DisplaySlot?.onSlotChange?.Invoke(DisplaySlot);
        }

        /*public static void StackItemToStack(DisplayItem fromStack, DisplayItem toStack)
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

        public static void StackItemToStack(DisplayItem fromStack, DisplayItem toStack, int amount)
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
        }*/
    }
}