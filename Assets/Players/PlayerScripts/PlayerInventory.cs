using System;
using System.Collections;
using Inventory;
using Items;
using Items.Data;
using UnityEngine;
using Zlipacket.CoreZlipacket.Misc;
using Environment = Zlipacket.CoreZlipacket.Misc.Environment;

namespace Players.PlayerScripts
{
    public class PlayerInventory : PlayerScript
    {
        public GrabInteractor grabInteractor;
        
        public DisplaySection handDisplay;
        public DisplaySlot[] handSlots => handDisplay.Slots;
        
        private void OnEnable()
        {
            playerInputMap.slot1 += Slot1;
            playerInputMap.slot2 += Slot2;
            playerInputMap.slot3 += Slot3;
        }

        private void OnDisable()
        {
            playerInputMap.slot1 -= Slot1;
            playerInputMap.slot2 -= Slot2;
            playerInputMap.slot3 -= Slot3;
        }

        private void Slot1(bool isHolding) => HandSlotHandle(0);
        private void Slot2(bool isHolding) => HandSlotHandle(1);
        private void Slot3(bool isHolding) => HandSlotHandle(2);
        
        private void HandSlotHandle(int slotNumber)
        {
            DisplaySlot displaySlot = handSlots[slotNumber];
            
            //Check if we hold anything.
            if (grabInteractor.grabObject != null)
            {
                //Check if the item is allow to store in inventory.
                if (!grabInteractor.itemGrab.allowInventoryStoring)
                    return;
                
                //Check if slot is empty.
                if (displaySlot.isEmpty)
                {
                    //Put it in corresponding hand slot.
                    displaySlot.AddItem(new InventoryItem(grabInteractor.itemGrab.data), true);
                    displaySlot.PlaySlotBounceAnimation();
                    grabInteractor.itemGrab.DestroyItem();
                }
                //Slot is not empty so we check if we can stack it.
                else if (DisplayItem.TryStackItem(grabInteractor.itemGrab.data, displaySlot.SlotDisplayItem.data))
                {
                    displaySlot.SlotDisplayItem.AddStack(1);
                    displaySlot.PlaySlotBounceAnimation();
                    grabInteractor.itemGrab.DestroyItem();
                }
                //Try put it in slot but the slot is full so we try to swap it if its unstackable or have 1 stack.
                else if (displaySlot.SlotDisplayItem.data.maxStack == 1 || displaySlot.SlotDisplayItem.data.stack == 1)
                {
                    //Swap the item with the item slot.
                    //Bug when swapping item to hand.
                    //Fixed by using Coroutine to separate destroy command.
                    StartCoroutine(SwappingItem(displaySlot));
                    displaySlot.PlaySlotBounceAnimation();
                }
                //If we are here, it means: 
                //We hold something, and we can't put it in the slot nor swap it.
                else
                    return;
            }
            //We did not grab anything means trying to bring item out.
            else
            {
                if (displaySlot.isEmpty)
                    return;
                
                //Slot not empty so we spawn things.
                SpawnItem(displaySlot.SlotDisplayItem.data);
                displaySlot.SlotDisplayItem.RemoveStack(1);
                displaySlot.PlaySlotBounceAnimation(false);
            }
        }

        private IEnumerator SwappingItem(DisplaySlot displaySlot)
        {
            Destroy(grabInteractor.grabObject);
            SpawnItem(displaySlot.SlotDisplayItem.data);
            //displaySlot.ClearSlot();
            
            yield return null;
            displaySlot.AddItem(new InventoryItem(grabInteractor.itemGrab.data), true);
        }
    }
}