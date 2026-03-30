using System;
using UnityEngine.UI;
using Zlipacket.CoreZlipacket.Tools;

namespace Zlipacket.CoreZlipacket.Audio.Script
{
    public class MixerScript : Singleton<MixerScript>
    {
        public Slider masterSlider;
        public Slider musicSlider;
        public Slider sfxSlider;
        public Slider voiceSlider;

        private void Start()
        {
            masterSlider.value = MixerManager.Instance.MasterVolume;
            musicSlider.value = MixerManager.Instance.MusicVolume;
            sfxSlider.value = MixerManager.Instance.SoundFXVolume;
            voiceSlider.value = MixerManager.Instance.VoiceVolume;
        }

        public void SetMasterVolume(float volume)
        {
            MixerManager.Instance.SetMasterVolume(volume);
        }

        public void SetMusicVolume(float volume)
        {
            MixerManager.Instance.SetMasterVolume(volume);
        }

        public void SetSoundFXVolume(float volume)
        {
            MixerManager.Instance.SetSoundFXVolume(volume);
        }
        
        public void SetVoiceVolume(float volume)
        {
            MixerManager.Instance.SetVoiceVolume(volume);
        }
    }
}