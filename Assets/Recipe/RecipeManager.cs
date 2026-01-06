using System;
using System.Collections.Generic;
using System.Linq;
using Items.Data;
using Recipe.Data;
using UnityEngine;
using Zlipacket.CoreZlipacket.Tools;

namespace Recipe
{
    public class RecipeManager : Singleton<RecipeManager>
    {
        public SO_RecipeList recipeDatabase;
        
        public SO_Recipe GetRecipe(string name) => recipeDatabase.GetRecipe(name);
        
        public SO_Recipe CheckRecipe(List<SO_Item> inputIngredients)
        {
            List<ItemData> ingredientsData = inputIngredients.Select(x => x.itemData).ToList();
            
            return CheckRecipe(ingredientsData);
        }
        
        public SO_Recipe CheckRecipe(List<ItemData> inputIngredients)
        {
            //Make a copy of list so we can check by removing item inside tempRecipe.
            List<SO_Item> tempRecipe = new();
            List<ItemData> tempIngredients = new();
            tempIngredients.AddRange(inputIngredients);
            tempIngredients.RemoveAll(x => x.ignoreInRecipeCheck); //Remove All ignore ingredients.
            
            //Loop to check all recipes in database.
            foreach (var recipe in recipeDatabase.recipes)
            {
                //Skip if ingredients not equal.
                if (recipe.ingredientsCount != tempIngredients.Count)
                    continue;
                
                //Populate with current recipe check.
                tempRecipe.AddRange(recipe.recipeData.recipeIngredients);
                
                //Loop tempIngredients and remove matching from tempRecipe.
                for (int i = 0; i < tempIngredients.Count; i++)
                {
                    SO_Item findItem = tempRecipe.Find(x => string.Equals(x.name, tempIngredients[i].nameId, StringComparison.InvariantCultureIgnoreCase));
                    if (findItem == null)
                        break;
                    else
                    {
                        tempRecipe.Remove(findItem);
                    }
                    
                    //If last loop for this recipe, and we removed all tempRecipe, 
                    //Return the matching recipe.
                    if (i == tempIngredients.Count - 1 && tempRecipe.Count == 0)
                        return recipe;
                }
                
                tempRecipe.Clear();
            }
            
            //If we are here, then no match were found.
            return null;
        }
    }
}