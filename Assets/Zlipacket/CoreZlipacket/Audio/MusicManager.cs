using System.Collections.Generic;
using UnityEngine;
using Zlipacket.CoreZlipacket.Tools;

namespace Zlipacket.CoreZlipacket.Audio
{
    public class MusicManager : Singleton<MusicManager>
    {
        [SerializeField] private AudioSource musicObject;

        private Dictionary<string, AudioSource> musicPlaylist = new();

        public bool CheckMusicAlreadyPlayed(string musicName) => musicPlaylist.ContainsKey(musicName);
        
        public void PlayMusic(AudioClip clip, float volume = 1f)
        {
            AudioSource music = Instantiate(musicObject, Instance.transform);
            
            music.clip = clip;
            music.volume = volume;
            music.loop = true;
            music.Play();
        }
        
        public void PlayMusicWithCallback(AudioClip clip, string callbackName, float volume = 1f)
        {
            if (CheckMusicAlreadyPlayed(callbackName))
            {
                Debug.Log($"Music \"{callbackName}\" already played");
                return;
            }
            
            AudioSource music = Instantiate(musicObject, Instance.transform);
            
            music.clip = clip;
            music.volume = volume;
            music.Play();
            
            musicPlaylist.Add(callbackName.ToLower(), music);
        }

        public void StopMusicByCallback(string callbackName)
        {
            if (musicPlaylist.TryGetValue(callbackName.ToLower(), out AudioSource music))
            {
                Destroy(music.gameObject);
            }
            else
            {
                Debug.LogWarning(callbackName + " not found in Music Stack.");
            }
        }

        public void StopAllMusic()
        {
            musicPlaylist.Clear();

            foreach (var music in ZlipUtilities.AllChilds(Instance.gameObject))
            {
                if (music != null)
                    Destroy(music.gameObject);
            }
        }

        public bool CheckIsSongPlaying(string callbackName)
        {
            if (musicPlaylist.TryGetValue(callbackName, out AudioSource music))
            {
                return music.isPlaying;
            }
            return false;
        }
    }
}