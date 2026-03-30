using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zlipacket.CoreZlipacket.Audio;
using Zlipacket.CoreZlipacket.Scene;
using Zlipacket.CoreZlipacket.Tools;

namespace Manager
{
    public class EndSceneManager : Singleton<TitleSceneManager>
    {
        public GameObject winImage;
        public GameObject loseImage;
        [SerializeField] private TextMeshProUGUI MoneyText;
        [SerializeField] private AudioClip WinSoundFX;
        [SerializeField] private AudioClip LoseSoundFX;

        public void Start()
        {
            OvercoffManager.Instance.endMoney -= OvercoffManager.Instance.tax;
            
            if (OvercoffManager.Instance.endMoney >= 0)
            {
                winImage.SetActive(true);
                loseImage.SetActive(false);
                if (WinSoundFX != null)
                    SoundFXManager.Instance.PlaySoundFX(WinSoundFX, transform, 1f);
            }
            else
            {
                winImage.SetActive(false);
                loseImage.SetActive(true);
                if (LoseSoundFX != null)
                    SoundFXManager.Instance.PlaySoundFX(LoseSoundFX, transform, 1f);
            }
            
            MoneyText.text = "Money: " + OvercoffManager.Instance.endMoney.ToString();
        }
    }
}