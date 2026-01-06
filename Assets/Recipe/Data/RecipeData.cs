using System;
using System.Collections.Generic;
using Items.Data;
using UnityEngine;

namespace Recipe.Data
{
    [Serializable]
    public class RecipeData
    {
        public string recipeName;
        public List<SO_Item> recipeIngredients;
        public SO_Item recipeResult;
        public List<RecipeTag> recipeTags;
        
        [Space]
        public RecipeDisplayData recipeDisplayData;
        
        [Space]
        [TextArea(15, 20)]
        public string description;
    }
    
    public enum RecipeTag
    {
        Coffee, 
        Tea, 
        Juice, 
        Hot, 
        Cold
    }
}