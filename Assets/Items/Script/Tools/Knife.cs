using Items.Script.Ingredients;
using UnityEngine;
using UnityEngine.Events;
using Zlipacket.CoreZlipacket.Audio;

namespace Items.Script.Tools
{
    public class Knife : ItemScript
    {
        public UnityEvent onCut;
        public AudioClip cutSound;
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out Cuttable cutable))
            {
                cutable.Cut();
                onCut?.Invoke();
                if (cutSound != null)
                    SoundFXManager.Instance.PlaySoundFXPosition(cutSound, cutable.transform.position);
            }
        }
    }
}