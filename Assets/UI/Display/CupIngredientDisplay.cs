using System.Collections.Generic;
using DG.Tweening;
using Items.Data;
using Players;
using UnityEngine;
using Zlipacket.CoreZlipacket.UI;

namespace UI.Display
{
    public class CupIngredientDisplay : ObjectDisplay<CupIngredientUI>
    {
        private List<SO_Item> ingredients;
        
        public CupIngredientDisplay(CupIngredientUI display, List<SO_Item> ingredients)
        {
            this.display = display;
            this.ingredients = ingredients;
            
            useShowHide = true;

            foreach (var ingredient in ingredients)
            {
                OnAddItem(ingredient);
            }
        }

        public void OnAddItem(SO_Item ingredient)
        {
            display.AddItemUI(ingredient);
        }

        public override void ShowUI(Player player)
        {
            base.ShowUI(player);

            display.OnShow();
        }

        public override void HideUI(Player player)
        {
            display.OnHide().onComplete += () =>
                base.HideUI(player);
        }

        protected override CupIngredientUI SpawnDisplay(UIManager ui)
        {
            return null;
        }
    }
}