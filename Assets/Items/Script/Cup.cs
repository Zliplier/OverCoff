using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Items.Data;
using Players;
using Recipe;
using Recipe.Data;
using UnityEngine;
using UnityEngine.Events;
using Environment = Zlipacket.CoreZlipacket.Misc.Environment;

namespace Items.Script
{
    public class Cup : ItemScript
    {
        private RecipeManager recipeManager => RecipeManager.Instance;
        private ItemManager itemManager => ItemManager.Instance;
        public List<SO_Item> containIngredients => data.containItems;

        public List<ItemTag> inputFilter;
        
        public UnityEvent onCombine;
        
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent(out Item item))
                return;
            if (!Item.CheckFilterTags(inputFilter, item))
                return;
            
            AddIngredient(item.item);
            
            item.DestroyItem();
        }

        public void AddIngredient(SO_Item addedItem)
        {
            containIngredients.Add(addedItem);
            OnCombineIngredients();
        }

        private void OnCombineIngredients()
        {
            SO_Recipe resultRecipe = recipeManager.CheckRecipe(containIngredients);
            
            //If we found any matching Recipe.
            if (resultRecipe != null)
                ChangeCup(resultRecipe.recipeData.recipeResult.itemData);
            //No matching Recipe so we revert the cup to the base appearance if needed.
            else
                ChangeCupToBase();
            
            onCombine?.Invoke();
        }

        private void ChangeCup(ItemData recipe)
        {
            //Check if the cup already the same as the target recipe if so we don't need to change it.
            if (string.Equals(recipe.nameId, data.nameId, StringComparison.InvariantCultureIgnoreCase))
                return;
            
            //Spawn the item at the same position and rotation.
            Item newCup = Instantiate(recipe.scriptableObject.itemPrefab, transform.position, transform.rotation)
                .GetComponent<Item>();
            
            //Initialize Default value.
            newCup.name = recipe.nameId;
            newCup.transform.SetParent(Environment.Instance.root);
            
            //Transfer contain item to that.
            newCup.itemData.containItems = new List<SO_Item>(containIngredients);
            
            //New Cup Animation.
            newCup.PlaySpawnAnimation();
            
            //Destroy this old cup.
            Destroy(gameObject);
        }

        private SO_Item FindBaseCup()
        {
            string baseCupName = "EmptyCup";
            
            if (containIngredients.Any(x => string.Equals(x.nameID, "Coffee", StringComparison.InvariantCultureIgnoreCase)))
                baseCupName = "CoffeeCup";
            else if (containIngredients.Any(x => string.Equals(x.nameID, "Milk", StringComparison.InvariantCultureIgnoreCase)))
                baseCupName = "MilkCup";
            else if (containIngredients.Any(x => string.Equals(x.nameID, "Soda", StringComparison.InvariantCultureIgnoreCase)))
                baseCupName = "SodaCup";
            
            SO_Item baseCup = itemManager.GetItemData(baseCupName, ItemManager.BASE_CUPS_LIST_ID);
            if (baseCup == null)
                Debug.LogError($"{baseCupName} could not be found.");
            
            return baseCup;
        }

        private void ChangeCupToBase()
        {
            //Find what base cup to use.
            SO_Item baseCup = FindBaseCup();
            
            //Check if the cup is already the target base cup.
            if (string.Equals(baseCup.itemData.nameId, data.nameId, StringComparison.InvariantCultureIgnoreCase))
                return;
            
            ChangeCup(baseCup.itemData);
        }
    }
}