using System;
using Players;
using UnityEngine;

namespace Items
{
    public class ItemScript : MonoBehaviour
    {
        [HideInInspector] public Item item;
        public ItemData.ItemData data => item.itemData;
        
        public Rigidbody rb => item.rb;

        private void Awake()
        {
            item = GetComponent<Item>();
        }
    }
}