using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Data
{
    [Serializable]
    public class ItemData
    {
        public string nameId;
        public string serializedId; //TODO: Change to Guid Id later when serialized.
        public List<ItemTag> itemTag;
        public int cost;
        public int stack;
        public int maxStack;
        public Sprite icon;
        public GameObject prefab;
        
        public ItemDisplayData displayData;
        
        [TextArea(15, 20)]
        public string description;
    }
}