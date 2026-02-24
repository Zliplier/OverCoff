using UnityEngine;
using UnityEngine.Audio;
using Zlipacket.CoreZlipacket.Tools;

namespace Zlipacket.CoreZlipacket.Audio
{
    public class MixerManager : Singleton<MixerManager>
    {
        [SerializeField] private AudioMixer audioMixer;
        
        [Header("Config")]
        public float MasterVolume = 0.7f;
        public float MusicVolume = 0.7f;
        public float SoundFXVolume = 0.7f;
        public float VoiceVolume = 0.7f;
        
        public void SetMasterVolume(float volume)
        {
            MasterVolume = volume;
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = volume;
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        }

        public void SetSoundFXVolume(float volume)
        {
            SoundFXVolume = volume;
            audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(volume) * 20);
        }
        
        public void SetVoiceVolume(float volume)
        {
            VoiceVolume = volume;
            audioMixer.SetFloat("VoiceVolume", Mathf.Log10(volume) * 20);
        }
    }
}