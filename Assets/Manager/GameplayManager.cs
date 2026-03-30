using System;
using Characters.Customer;
using Level;
using TMPro;
using UnityEngine;
using Zlipacket.CoreZlipacket.Audio;
using Zlipacket.CoreZlipacket.Tools;

namespace Manager
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        public SO_LevelConfig levelConfig;
        public TextMeshProUGUI taxText;
        public AudioClip gameplayMusic;

        private void Start()
        {
            if (taxText != null)
                taxText.text = levelConfig.tax.ToString();
            
            if (gameplayMusic != null)
                MusicManager.Instance.PlayMusicWithCallback(gameplayMusic, gameplayMusic.name);
        }

        private void OnDestroy()
        {
            if (gameplayMusic != null)
                MusicManager.Instance.StopAllMusic();
        }

        public void SpawnCustomer(GameObject customer)
        {
            CustomerManager.Instance.SpawnCustomer(customer);
        }
    }
}