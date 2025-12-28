using System.Collections.Generic;
using UnityEngine;

namespace Recipe.Data
{
    [CreateAssetMenu(fileName = "Recipe List", menuName = "Recipe/RecipeList")]
    public class SO_RecipeList : ScriptableObject
    {
        public List<SO_Recipe> recipes;
        public SO_Recipe GetRecipe(string name) 
            => recipes.Find(
                x => string.Equals(x.name, name, System.StringComparison.InvariantCultureIgnoreCase));
    }
}