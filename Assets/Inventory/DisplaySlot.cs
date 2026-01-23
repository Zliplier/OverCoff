using System;
using System.Collections.Generic;
using DG.Tweening;
using Items.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zlipacket.CoreZlipacket.Tools;

namespace Inventory
{
    public class DisplaySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public InventoryManager manager { get; private set; }
        public InventorySection section { get; private set; }
        public int slotIndex { get; private set; }
        public InventoryItem slotItem
        {
            get => section.sectionInventory[slotIndex];
            set => section.sectionInventory[slotIndex] = value;
        }

        public DisplayItem PointerDisplayItem => manager.pointerDisplayItem;
        public DisplayItem SlotDisplayItem => (transform.childCount != 0)? transform.GetChild(0).GetComponent<DisplayItem>() : null;

        public Image image;
        
        public bool isEmpty => transform.childCount == 0;

        public UnityEvent<DisplaySlot> onSlotChange;
        
        private Tween slotAnimation = null;
        public bool isTweening => slotAnimation != null;
        
        [Header("Configs")]
        public Color deSelectedColor;
        public Color selectedColor;
        
        public void Initialize(InventorySection section, InventoryManager manager, int slotIndex)
        {
            this.section = section;
            this.manager = manager;
            this.slotIndex = slotIndex;
            
            if (slotItem.amount <= 0)
            {
                ClearSlot(false);
            }
            else
            {
                ClearSlot(false);
                CreateItemDisplay(slotItem);
            }
        }

        private void OnEnable()
        {
            image.color = deSelectedColor;
        }

        private void OnDisable()
        {
            image.color = deSelectedColor;
            transform.localScale = Vector3.one;
        }

        public void SyncSlot()
        {
            if (slotItem.amount <= 0)
            {
                ClearSlot(false, false);
            }
            else
            {
                if (isEmpty)
                    CreateItemDisplay(slotItem);
                else
                    SlotDisplayItem.SyncItem();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //Left Click
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                LeftClick();
            }
            //Right Click
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                RightClick();
            }
            
            PlaySlotBounceAnimation(false);
        }

        private void LeftClick()
        {
            //Check if cursor hold something.
            if (PointerDisplayItem != null)
            {
                //Check if this slot is empty to put item down.
                if (isEmpty)
                {
                    DropItem();
                    return;
                }
                
                //If slot not empty check if we can stack the item.
                if (DisplayItem.TryStackItem(PointerDisplayItem.data, SlotDisplayItem.data))
                {
                    //StackItemToStack(manager.pointerDisplayItem, SlotDisplayItem);
                    StackAll(PointerDisplayItem);
                }
                //If Item cannot be stacked, we swap it.
                else
                {
                    SwapItem();
                }
            }
            //The cursor is empty, therefore check if this slot has item to drag.
            else if (!isEmpty)
            {
                DragItem();
            }
        }

        private void RightClick()
        {
            //Check if cursor hold something.
            if (PointerDisplayItem != null)
            {
                //Check if this slot is empty to put 1 stack down.
                if (isEmpty)
                {
                    //Put 1 stack down.
                    if (PointerDisplayItem.InventoryItem.stack == 1)
                    {
                        DropItem();
                        return;
                    }
                    slotItem = manager.pointerDisplayItem.InventoryItem.Copy();
                    slotItem.stack = 1;
                    PointerDisplayItem.RemoveStack(1);
                    onSlotChange?.Invoke(this);
                    return;
                }
                
                //If slot not empty check if we can stack the item by 1 stack.
                if (DisplayItem.TryStackItem(PointerDisplayItem.data, SlotDisplayItem.data))
                {
                    //Stack Items by 1.
                    SlotDisplayItem.AddStack(1);
                    PointerDisplayItem.RemoveStack(1);
                }
                //If Item cannot be stacked, we swap it.
                else
                {
                    SwapItem();
                }
            }
            //The cursor is empty, therefore check if this slot has item to drag.
            else if (!isEmpty)
            {
                //TODO: Drag half the stack out (Round Up).
                //Or just pick it up instead.
                DragItem();
            }
        }
        
        private void DragItem()
        {
            DisplayItem displayItem = SlotDisplayItem;
            
            displayItem.InventoryItem = slotItem.Copy();
            
            displayItem.StartDrag();
            manager.pointerDisplayItem = displayItem;
            displayItem.DisplaySlot = null;
            displayItem.transform.SetParent(manager.root);
            displayItem.transform.SetAsLastSibling();
            
            slotItem.Clear();
            
            onSlotChange?.Invoke(this);
        }
        
        private void DropItem()
        {
            PointerDisplayItem.parentAfterDrag = transform;
            PointerDisplayItem.EndDrag();
            PointerDisplayItem.DisplaySlot = this;
            
            slotItem = manager.pointerDisplayItem.InventoryItem.Copy();
            manager.pointerDisplayItem = null;
            
            onSlotChange?.Invoke(this);
        }

        private DisplayItem CreateItemDisplay(InventoryItem item)
        {
            DisplayItem newDisplayItem = Instantiate(
                //Get prefab by loading from Resources.
                Resources.Load(InventoryManager.INVENTORY_ITEM_PREFAB_PATH), transform).GetComponent<DisplayItem>();
            newDisplayItem.name = "Item";
            newDisplayItem.Initialize(ref item, this);
            
            return newDisplayItem;
        }
        
        private void SwapItem()
        {
            DisplayItem displayItem = SlotDisplayItem;
            
            //Get current item out.
            displayItem.StartDrag();
            displayItem.transform.SetParent(manager.root);
            displayItem.transform.SetAsLastSibling();
            
            //Bring new Item in.
            manager.pointerDisplayItem.parentAfterDrag = transform;
            manager.pointerDisplayItem.EndDrag();
            
            //Reset pointer.
            manager.pointerDisplayItem = displayItem;
            
            onSlotChange?.Invoke(this);
        }

        /*private void Stack(InventoryItem item, int amount)
        {
            int toStackCapacity = slotItem.maxStack - slotItem.stack;
            int transferStack;

            //Check if he amount to transfer is greater than the capacity toStack can hold.
            if (amount > toStackCapacity)
                amount = toStackCapacity;

            //fromStack is not enough to the amount so we transfer all of fromStack as much as we can.
            if (amount >= item.stack)
                transferStack = item.stack;
            //fromStack is enough so we transfer the amount.
            else
                transferStack = amount;

            SlotDisplayItem.AddStack(transferStack);
            PointerDisplayItem.RemoveStack(transferStack);
        }*/

        private void StackAll(DisplayItem fromStack)
        {
            int toStackCapacity = slotItem.maxStack - slotItem.stack;
            int transferStack;

            //Check if fromStack is less than or equal to toStack capacity so we can move the entire fromStack.
            if (fromStack.InventoryItem.stack <= toStackCapacity)
                transferStack = fromStack.InventoryItem.stack;
            //fromStack is greater than toStack so after the transfer the fromStack will have some left.
            else
                transferStack = toStackCapacity;

            SlotDisplayItem.AddStack(transferStack);
            fromStack.RemoveStack(transferStack);
        }

        public void AddItem(InventoryItem item, bool overwrite = false)
        {
            if (!overwrite && !isEmpty)
                return;
            
            ClearSlot();
            slotItem = item;
            onSlotChange?.Invoke(this);
        }

        public void ClearSlot(bool clearDataCallback = true, bool syncCallback = true)
        {
            if (isEmpty)
                return;
            
            Destroy(SlotDisplayItem.gameObject);

            if (clearDataCallback)
                slotItem.Clear();
            
            if (syncCallback)
                onSlotChange?.Invoke(this);
        }
        
        public void PlaySlotBounceAnimation(bool scaleOutward = true, float duration = 0.3f)
        {
            if (isTweening)
                slotAnimation?.Kill();
            
            slotAnimation = transform.DOScale(scaleOutward ? 1.1f : 0.9f, duration / 2);
            
            slotAnimation.onComplete += () => 
                slotAnimation = transform.DOScale(1f, duration / 2);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
             image.color = selectedColor;
             if (isTweening)
                 slotAnimation?.Kill();
             slotAnimation = transform.DOScale(1.1f, 0.3f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            image.color = deSelectedColor;
            if (isTweening)
                slotAnimation?.Kill();
            slotAnimation = transform.DOScale(1f, 0.3f);
        }
    }
}