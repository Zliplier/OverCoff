using DG.Tweening;
using UI;
using UnityEngine;

namespace Players.PlayerScripts
{
    public class PlayerBook : PlayerScript
    {
        public const string BOOK_SECTION = "Book";
        
        [SerializeField] private GameObject pageRoot;
        
        public UISection bookSection => UIManager.GetUISection(BOOK_SECTION);
        
        public bool isBookOpen => bookSection.sectionRoot.activeInHierarchy;
        
        public BookType currentBook;
        
        private Tween bookAnimation = null;
        public bool isTweening => bookAnimation != null;
        
        public RecipeBook recipeBook;
        public TutorialBook tutorialBook;
        
        private void OnEnable()
        {
            playerInputMap.recipeEvent += OpenRecipe;
            playerInputMap.tutorialEvent += OpenTutorial;
            
            uiInputMap.cancelEvent += CloseBook;
        }

        private void OnDisable()
        {
            playerInputMap.recipeEvent -= OpenRecipe;
            playerInputMap.tutorialEvent -= OpenTutorial;
            
            uiInputMap.cancelEvent -= CloseBook;
        }
        
        public void OpenRecipe(bool _) => OpenBook(BookType.RecipeBook);
        public void OpenTutorial(bool _) => OpenBook(BookType.TutorialBook);
        
        public void OpenBook(BookType page)
        {
            player.SetCursorLockState(false);
            
            bookSection.sectionRoot.SetActive(true);
            currentBook = page;
            
            switch (page)
            {
                case BookType.RecipeBook:
                    
                    break;
                case BookType.TutorialBook:
                    tutorialBook.Open();
                    break;
            }

            PlayPopUpAnimation(true).onComplete += () =>
            {
                playerInputMap.SetMapEnable(false);
                uiInputMap.SetMapEnable(true);
            };
        }

        public void CloseBook(bool isHolding)
        {
            if (!isBookOpen)
                return;
            
            switch (currentBook)
            {
                case BookType.RecipeBook:
                    
                    break;
                case BookType.TutorialBook:
                    tutorialBook.Close();
                    break;
            }

            PlayPopUpAnimation(false).onComplete += () =>
            {
                player.SetCursorLockState(true);

                playerInputMap.SetMapEnable(true);
                uiInputMap.SetMapEnable(false);

                bookSection.sectionRoot.SetActive(false);
            };
        }

        private Tween PlayPopUpAnimation(bool isOpen)
        {
            if (isTweening)
                bookAnimation.Kill();

            if (isOpen)
            {
                pageRoot.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                bookAnimation = pageRoot.transform.DOScale(1f, 0.2f);
            }
            else
            {
                bookAnimation = pageRoot.transform.DOScale(0.8f, 0.1f);
            }
            
            return bookAnimation;
        }
    }

    public enum BookType
    {
        None, 
        RecipeBook, 
        TutorialBook
    }
}