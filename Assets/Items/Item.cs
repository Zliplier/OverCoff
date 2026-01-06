using System;
using System.Collections.Generic;
using Items.Data;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Collider))]
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
            
            //TODO: Logic Update when creating this for some reason.
        }
        
        public static bool CheckFilterTags(List<ItemTag> inputFilter, Item item, bool checkAll = false)
        {
            foreach (var filter in inputFilter)
            {
                //Might have some bugs man.
                if (item.item.itemTag.Contains(filter))
                    return true;
                if (checkAll)
                    return false;
            }
            
            return false;
        }
    }
}