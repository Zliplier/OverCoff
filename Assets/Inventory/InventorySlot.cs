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
    public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public InventoryManager manager { get; private set; }
        public InventoryItem pointerItem => manager.pointerItem;
        public InventoryItem slotItem => (transform.childCount != 0)? transform.GetChild(0).GetComponent<InventoryItem>() : null;

        public Image image;
        public bool enableSlotSync;
        [DrawIf("enableSlotSync")] public List<InventorySlot> syncSlots;
        
        public bool isEmpty => transform.childCount == 0;

        public UnityEvent<InventoryItem> onItemChange;
        
        private Tween slotAnimation = null;
        public bool isTweening => slotAnimation != null;
        
        [Header("Configs")]
        public Color deSelectedColor;
        public Color selectedColor;

        private void Awake()
        {
            if (enableSlotSync)
                onItemChange.AddListener(SyncSlot);
        }

        public void Initialize(InventoryManager manager)
        {
            this.manager = manager;

            if (!isEmpty)
                slotItem.Initialize(null, this);
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
            if (pointerItem != null)
            {
                //Check if this slot is empty to put item down.
                if (isEmpty)
                {
                    DropItem();
                    return;
                }
                
                //If slot not empty check if we can stack the item.
                if (InventoryItem.TryStackItem(pointerItem.data, slotItem.data))
                {
                    StackItemToStack(manager.pointerItem, slotItem);
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
            if (pointerItem != null)
            {
                //Check if this slot is empty to put 1 stack down.
                if (isEmpty)
                {
                    //Put 1 stack down.
                    AddItem(pointerItem.data, 1);
                    pointerItem.RemoveStack(1);
                    return;
                }
                
                //If slot not empty check if we can stack the item by 1 stack.
                if (InventoryItem.TryStackItem(pointerItem.data, slotItem.data))
                {
                    //Stack Items by 1.
                    StackItemToStack(manager.pointerItem, slotItem, 1);
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
            }
        }
        
        private void DragItem()
        {
            InventoryItem item = slotItem;
            
            item.StartDrag();
            manager.pointerItem = item;
            item.transform.SetParent(manager.root);
            item.transform.SetAsLastSibling();
            
            onItemChange?.Invoke(null);
        }
        
        private void DropItem()
        {
            manager.pointerItem.parentAfterDrag = transform;
            manager.pointerItem.EndDrag();
            manager.pointerItem = null;
            
            onItemChange?.Invoke(slotItem);
        }

        private void SwapItem()
        {
            InventoryItem item = slotItem;
            
            //Get current item out.
            item.StartDrag();
            item.transform.SetParent(manager.root);
            item.transform.SetAsLastSibling();
            
            //Bring new Item in.
            manager.pointerItem.parentAfterDrag = transform;
            manager.pointerItem.EndDrag();
            
            //Reset pointer.
            manager.pointerItem = item;
            
            onItemChange?.Invoke(slotItem);
        }
        
        public void StackItemToStack(InventoryItem fromStack, InventoryItem toStack)
        {
            InventoryItem.StackItemToStack(fromStack, toStack);
            onItemChange?.Invoke(slotItem);
        }
        
        public void StackItemToStack(InventoryItem fromStack, InventoryItem toStack, int amount)
        {
            InventoryItem.StackItemToStack(fromStack, toStack, amount);
            onItemChange?.Invoke(slotItem);
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

        public void IncrementItem(int amount, bool callItemChanged = true)
        {
            if (isEmpty)
                return;
            
            slotItem.AddStack(amount);
            
            if (callItemChanged)
                onItemChange?.Invoke(slotItem);
        }
        
        public void AddItem(ItemData item, bool isOverride = false, bool callItemChanged = true)
        {
            if (!isOverride && !isEmpty)
                return;
            
            if (isOverride && !isEmpty)
                RemoveItem(-1, false);
            
            InventoryItem newItem = Instantiate(
                //Get prefab by loading from Resources.
                Resources.Load(InventoryManager.INVENTORY_ITEM_PREFAB_PATH), transform).GetComponent<InventoryItem>();
            newItem.name = "Item";
            newItem.Initialize(item, this);
            
            if (callItemChanged)
                onItemChange?.Invoke(newItem);
        }

        public void AddItem(ItemData item, int totalAmount, bool isOverride = false, bool callItemChanged = true)
        {
            if (!isOverride && !isEmpty)
                return;
            
            if (isOverride && !isEmpty)
                RemoveItem(-1, false);
            
            InventoryItem newItem = Instantiate(
                //Get prefab by loading from Resources.
                Resources.Load(InventoryManager.INVENTORY_ITEM_PREFAB_PATH), transform).GetComponent<InventoryItem>();
            newItem.name = "Item";
            newItem.Initialize(item, this);
            newItem.stack = totalAmount;
            
            if (callItemChanged)
                onItemChange?.Invoke(newItem);
        }
        
        public void RemoveItem(int count, bool callItemChanged = true)
        {
            if (isEmpty)
                return;
            
            //If -1, we remove all item regardless of stack. Just trash it completely.
            if (count < 0)
            {
                Destroy(slotItem.gameObject);
            }
            //If not remove by the amount of stack.
            else
            {
                slotItem.RemoveStack(count);
            }
            
            if (callItemChanged)
                onItemChange?.Invoke(slotItem);
        }
        
        public void SyncSlot(InventoryItem item)
        {
            if (syncSlots == null)
                return;

            foreach (var slot in syncSlots)
            {
                if (slot == null)
                    continue;

                if (item != null)
                {
                    if (item.stack >= 1)
                        slot.AddItem(item.data, true, false);
                    else
                        slot.RemoveItem(-1, false);
                }
                else
                    slot.RemoveItem(-1, false);
            }
        }

        public void PlaySlotBounceAnimation(bool scaleOutward = true, float duration = 0.3f)
        {
            if (isTweening)
                slotAnimation?.Kill();
            
            slotAnimation = transform.DOScale(scaleOutward ? 1.1f : 0.9f, duration / 2);
            
            slotAnimation.onComplete += () => 
                slotAnimation = transform.DOScale(1f, duration / 2);
        }
    }
}