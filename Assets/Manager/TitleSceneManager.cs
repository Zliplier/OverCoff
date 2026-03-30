using System;
using UnityEngine;
using Zlipacket.CoreZlipacket.Audio;
using Zlipacket.CoreZlipacket.Tools;

namespace Manager
{
    public class TitleSceneManager : Singleton<TitleSceneManager>
    {
        public AudioClip titleMusic;

        private void Start()
        {
            if (titleMusic != null)
                MusicManager.Instance.PlayMusicWithCallback(titleMusic, titleMusic.name);
        }

        private void OnDestroy()
        {
            if (titleMusic != null)
                MusicManager.Instance.StopAllMusic();
        }
    }
}