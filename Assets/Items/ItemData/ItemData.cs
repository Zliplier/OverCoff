using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ItemData
{
    [Serializable]
    public class ItemData
    {
        public string serializedId; //TODO: Change to Guid Id later when serialized.
        public List<ItemTag> itemTag;
        public int cost;
        public int stack;
        public int maxStack;
        public Sprite icon;
        public GameObject prefab;
    }
}