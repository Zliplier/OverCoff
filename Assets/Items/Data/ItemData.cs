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
        
        [Space]
        public ItemDisplayData displayData;
        
        [Space]
        [TextArea(15, 20)]
        public string description;

        [Space]
        public bool ignoreInRecipeCheck;
        
        public ItemData(ItemData data)
        {
            nameId = data.nameId;
            serializedId = data.serializedId;
            itemTag = new List<ItemTag>();
            cost = data.cost;
            stack = data.stack;
            maxStack = data.maxStack;
            icon = data.icon;
            prefab = data.prefab;
            displayData = data.displayData;
            description = data.description;
        }
    }
    
    public enum ItemTag
    {
        Grabable, 
        Ingredient, 
        RecipeResult, 
        Furniture, 
        Fryable, 
        Cuttable, 
        Condiment
    }
}