using System;
using UnityEngine;

namespace Zlipacket.CoreZlipacket.Audio.Script
{
    public class MusicScript : MonoBehaviour
    {
        public AudioClip audioClip;
        public float volume = 1f;
        public bool playOnStart = false;

        private void Start()
        {
            if (playOnStart)
                PlaySongOverride();
        }

        public void PlaySongOverride()
        {
            MusicManager.Instance.StopAllMusic();
            MusicManager.Instance.PlayMusic(audioClip, volume);
        }
    }
}