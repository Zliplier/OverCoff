using System;
using Items.Data;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Item : MonoBehaviour
    {
        public SO_Item item;
        public ItemData itemData => item.itemData; //TODO: Fix later when save/load.
        
        public Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
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
    }
}