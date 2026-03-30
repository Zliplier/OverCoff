using UnityEngine;

namespace Zlipacket.CoreZlipacket.Audio.Script
{
    public class SfxScript : MonoBehaviour
    {
        public AudioClip audioClip;
        public float volume = 1f;

        public void PlayClip()
        {
            if (audioClip != null)
                SoundFXManager.Instance.PlaySoundFX(audioClip, SoundFXManager.Instance.transform, volume);
        }
    }
}