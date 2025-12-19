using Items.ItemData;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour
    {
        public SO_ItemData ItemData;
    }

    public enum ItemTag
    {
        Ingredient, 
        RecipeResult, 
        Furniture, 
        Fryable, 
        Cuttable,
    }
}