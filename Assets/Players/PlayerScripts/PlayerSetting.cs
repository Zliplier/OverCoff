using DG.Tweening;
using UI;
using UnityEngine;

namespace Players.PlayerScripts
{
    public class PlayerSetting : PlayerScript
    {
        public const string SETTING_SECTION = "Setting";
        
        [SerializeField] private GameObject pageRoot;
        
        public UISection settingSection => UIManager.GetUISection(SETTING_SECTION);
        
        public bool isBookOpen => settingSection.sectionRoot.activeInHierarchy;
        
        private Tween tw_Animation = null;
        public bool isTweening => tw_Animation != null;
        
        private void OnEnable()
        {
            uiInputMap.cancelEvent += CloseSetting;
        }

        private void OnDisable()
        {
            uiInputMap.cancelEvent -= CloseSetting;
        }
        
        public void OpenSetting()
        {
            player.SetCursorLockState(false);
            
            settingSection.sectionRoot.SetActive(true);

            PlayPopUpAnimation(true).onComplete += () =>
            {
                playerInputMap.SetMapEnable(false);
                uiInputMap.SetMapEnable(true);
            };
        }

        public void CloseSetting(bool isHolding)
        {
            if (!isBookOpen)
                return;

            PlayPopUpAnimation(false).onComplete += () =>
            {
                player.SetCursorLockState(true);

                playerInputMap.SetMapEnable(true);
                uiInputMap.SetMapEnable(false);
                
                settingSection.sectionRoot.SetActive(false);
            };
        }

        private Tween PlayPopUpAnimation(bool isOpen)
        {
            if (isTweening)
                tw_Animation.Kill();

            if (isOpen)
            {
                pageRoot.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                tw_Animation = pageRoot.transform.DOScale(1f, 0.2f);
            }
            else
            {
                tw_Animation = pageRoot.transform.DOScale(0.8f, 0.1f);
            }
            
            return tw_Animation;
        }
    }
}