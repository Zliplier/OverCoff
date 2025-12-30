using System;
using Inventory;
using Items;
using Items.Data;
using Players.UI;
using UnityEngine;

namespace Players.PlayerScripts
{
    public class PlayerInventory : PlayerScript
    {
        public GrabInteractor grabInteractor;
        
        public InventorySlot[] handSlot;
        public InventoryManager playerInventory;

        [Header("Config")]
        public float defaultSpawningDistance = 0.5f;
        
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

        private void HandSlotHandle(int slotNumber)
        {
            InventorySlot slot = handSlot[slotNumber];
            
            //Check if slot is empty.
            if (slot.isEmpty)
            {
                //Check if we hold anything.
                if (grabInteractor.grabObject != null)
                {
                    //Put it in corresponding hand slot.
                    slot.AddItem(grabInteractor.itemGrab.data);
                    Destroy(grabInteractor.grabObject);
                }
            }
            //If not empty, bring the item out by 1.
            else
            {
                InventoryItem slotItem = slot.slotItem;
                
                SpawnItem(slotItem.item);
                
                slot.RemoveItem(1);
            }
        }

        private void SpawnItem(ItemData itemData)
        {
            Vector3 spawnPosition = cam.transform.position + (cam.transform.forward * defaultSpawningDistance);
            
            //Check for obstruction.
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, 
                    out RaycastHit hit, defaultSpawningDistance))
                spawnPosition = hit.point;
            
            //Spawning Object Item.
            GameObject newItem = Instantiate(itemData.scriptableObject.itemPrefab, spawnPosition, Quaternion.identity);
            newItem.GetComponent<Item>().Initialize(itemData);
        }
    }
}