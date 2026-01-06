using System;
using System.Collections.Generic;
using Items.Data;
using Players;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Item))]
    public abstract class ItemScript : MonoBehaviour
    {
        [HideInInspector] public Item item;
        public ItemData data => item.itemData;
        
        public Rigidbody rb => item.rb;

        protected virtual void Awake()
        {
            item = GetComponent<Item>();
        }
    }
}