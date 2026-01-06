using System;
using System.Collections.Generic;
using Items.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Items.Script
{
    public class ItemHolderTrigger : MonoBehaviour
    {
        public List<ItemTag> inputFilterTag;

        [SerializeField] private Transform holdArea;
        
        [HideInInspector] public Item containItem;
        public bool isEmpty => containItem == null;
        
        [Header("Events")]
        public UnityEvent onContain;
        public UnityEvent onUnContain;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out Item item))
                return;
            
            if (!Item.CheckFilterTags(inputFilterTag, item))
                return;
            
            //If the holder is empty then we contain it.
            if (isEmpty)
                ContainItem(item);
            //However, if not we try to swap place with it.
            else
                SwapItem(item);
            
            onContain?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (isEmpty)
                return;
            
            if (!other.gameObject.TryGetComponent(out Item item))
                return;
            
            if (!Item.CheckFilterTags(inputFilterTag, item))
                return;
            
            if (item == containItem)
                containItem = null;
            
            onUnContain?.Invoke();
        }
        
        private void ContainItem(Item item)
        {
            //Put Item out of player hand.
            if (item.TryGetComponent(out ItemGrab grab))
                grab.Reset();
            
            //Set transform
            item.transform.position = holdArea.position;
            item.transform.rotation = holdArea.rotation;
            
            item.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            
            containItem = item;
        }

        private void SwapItem(Item item)
        {
            //Put Item out of player hand.
            if (item.TryGetComponent(out ItemGrab grab))
                grab.Reset();
            
            //Set transform of contain Item.
            containItem.transform.position = item.transform.position;
            containItem.transform.rotation = item.transform.rotation;
            //Set transform of new Item
            item.transform.position = holdArea.position;
            item.transform.rotation = holdArea.rotation;
            
            item.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            
            containItem = item;
        }
    }
}