using TMPro;
using UnityEngine;
using Zlipacket.CoreZlipacket.Audio;
using Zlipacket.CoreZlipacket.Scene;
using Zlipacket.CoreZlipacket.Tools;

namespace Manager
{
    public class EndSceneManager : Singleton<TitleSceneManager>
    {
        [SerializeField] private TextMeshProUGUI WinLoseText;
        [SerializeField] private TextMeshProUGUI MoneyText;
        [SerializeField] private AudioClip WinSoundFX;
        [SerializeField] private AudioClip LoseSoundFX;

        public void Start()
        {
            OvercoffManager.Instance.endMoney -= OvercoffManager.Instance.tax;
            
            if (OvercoffManager.Instance.endMoney >= 0)
            {
                WinLoseText.text = "Win";
                if (WinSoundFX != null)
                    SoundFXManager.Instance.PlaySoundFX(WinSoundFX, transform, 1f);
            }
            else
            {
                WinLoseText.text = "Lose";
                if (LoseSoundFX != null)
                    SoundFXManager.Instance.PlaySoundFX(LoseSoundFX, transform, 1f);
            }
            
            MoneyText.text = "Money: " + OvercoffManager.Instance.endMoney.ToString();
        }
    }
}