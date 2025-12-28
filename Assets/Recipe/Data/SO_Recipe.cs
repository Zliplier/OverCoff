using UnityEngine;

namespace Recipe.Data
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Recipe")]
    public class SO_Recipe : ScriptableObject
    {
        public RecipeData recipeData;
        public int ingredientsCount => recipeData.recipeIngredients.Count;
    }
}