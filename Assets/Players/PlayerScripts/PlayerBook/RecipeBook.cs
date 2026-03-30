using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Players.PlayerScripts
{
    public class RecipeBook : PlayerScript
    {
        public const string RECIPE_PANEL = "Recipe";
        
        public Panel recipePanel => UIManager.GetPanel(PlayerBook.BOOK_SECTION, RECIPE_PANEL);
        
        public List<GameObject> pages;
        private int currentPage;
        
        public void Open()
        {
            recipePanel.panelRoot.SetActive(true);
            currentPage = 0;
            pages[currentPage].SetActive(true);
        }

        public void Close()
        {
            recipePanel.panelRoot.SetActive(false);
            pages[currentPage].SetActive(false);
        }

        public void ChangePage(bool isRight)
        {
            currentPage += isRight ? 1 : -1;
            if (currentPage >= pages.Count)
                currentPage--;
            else if (currentPage < 0)
                currentPage = 0;
            
            foreach (var page in pages)
                page.SetActive(false);
            
            pages[currentPage].SetActive(true);
        }
    }
}