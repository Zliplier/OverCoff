using System;
using Items.Data;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Item : MonoBehaviour
    {
        public SO_Item item;
        public ItemData itemData;
        
        public Rigidbody rb;

        private void Awake()
        {
            if (string.IsNullOrWhiteSpace(itemData.nameId))
                itemData = new ItemData(item.itemData);
            
            rb = GetComponent<Rigidbody>();
        }

        public void Initialize(ItemData itemData)
        {
            this.itemData = new ItemData(itemData);
            
            //TODO: Logic Update when creating this.
        }
    }
}