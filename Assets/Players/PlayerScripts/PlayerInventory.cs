using System;
using System.Collections;
using Inventory;
using Items;
using Items.Data;
using Players.UI;
using UnityEngine;
using Zlipacket.CoreZlipacket.Misc;
using Environment = Zlipacket.CoreZlipacket.Misc.Environment;

namespace Players.PlayerScripts
{
    public class PlayerInventory : PlayerScript
    {
        public GrabInteractor grabInteractor;
        
        public InventorySlot[] handSlot;
        public InventoryManager playerInventory;
        
        private void OnEnable()
        {
            playerInputMap.slot1 += Slot1;
            playerInputMap.slot2 += Slot2;
            playerInputMap.slot3 += Slot3;
            playerInputMap.inventoryEvent += OpenInventory;
        }

        private void OnDisable()
        {
            playerInputMap.slot1 -= Slot1;
            playerInputMap.slot2 -= Slot2;
            playerInputMap.slot3 -= Slot3;
            playerInputMap.inventoryEvent -= OpenInventory;
        }

        private void OpenInventory(bool isHolding)
        {
            player.playerBook.OpenBook(BookPage.Inventory);
        }

        private void Slot1(bool isHolding) => HandSlotHandle(0);
        private void Slot2(bool isHolding) => HandSlotHandle(1);
        private void Slot3(bool isHolding) => HandSlotHandle(2);

        //Might have some bugs, idk. Too tired to test this shit.
        private void HandSlotHandle(int slotNumber)
        {
            InventorySlot slot = handSlot[slotNumber];
            
            //Check if we hold anything.
            if (grabInteractor.grabObject != null)
            {
                //Check if slot is empty.
                if (slot.isEmpty)
                {
                    //Put it in corresponding hand slot.
                    slot.AddItem(grabInteractor.itemGrab.data);
                    Destroy(grabInteractor.grabObject);
                }
                //Slot is not empty so we check if we can stack it.
                else if (InventoryItem.TryStackItem(grabInteractor.itemGrab.data, slot.slotItem.data))
                {
                    slot.AddItem(1);
                    Destroy(grabInteractor.grabObject);
                }
                //Try put it in slot but the slot is full so we try to swap it if its unstackable or have 1 stack.
                else if (slot.slotItem.maxStack == 1 || slot.slotItem.stack == 1)
                {
                    //Swap the item with the item slot.
                    //Bug when swapping item to hand.
                    //Fixed by using Coroutine to separate destroy command.
                    StartCoroutine(SwappingItem(slot));
                }
                //If we are here, it means: 
                //We hold something, and we can't put it in the slot nor swap it.
                else
                    return;
            }
            //We did not grab anything means trying to bring item out.
            else
            {
                if (slot.isEmpty)
                    return;
                
                //Slot not empty so we spawn things.
                SpawnItem(slot.slotItem.data);
                slot.RemoveItem(1);
            }
        }

        private IEnumerator SwappingItem(InventorySlot slot)
        {
            SpawnItem(slot.slotItem.data);
            slot.RemoveItem(-1);
            yield return null;
            slot.AddItem(grabInteractor.itemGrab.data);
            Destroy(grabInteractor.grabObject);
        }
    }
}