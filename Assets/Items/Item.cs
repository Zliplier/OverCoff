using System;
using Items.ItemData;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Item : MonoBehaviour
    {
        public SO_Item item;

        public Rigidbody rb;

        private void Start()
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