using Items.Script.Ingredients;
using UnityEngine;
using UnityEngine.Events;

namespace Items.Script.Tools
{
    public class Knife : ItemScript
    {
        public UnityEvent onCut;
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out Cuttable cutable))
            {
                cutable.Cut();
                onCut?.Invoke();
            }
        }
    }
}