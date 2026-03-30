using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Players.PlayerScripts
{
    public class TutorialBook : PlayerScript
    {
        public const string TUTORIAL_PANEL = "Tutorial";
        
        public Panel tutorialPanel => UIManager.GetPanel(PlayerBook.BOOK_SECTION, TUTORIAL_PANEL);
        
        public List<GameObject> pages;
        private int currentPage;
        
        public void Open()
        {
            tutorialPanel.panelRoot.SetActive(true);
            currentPage = 0;
            pages[currentPage].SetActive(true);
        }

        public void Close()
        {
            tutorialPanel.panelRoot.SetActive(false);
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