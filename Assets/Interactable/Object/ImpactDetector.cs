using UnityEngine;
using UnityEngine.Events;

namespace Interactable.Object
{
    public class ImpactDetector : MonoBehaviour
    {
        [Header("Configs")]
        public float ImpactThreshold = 0.1f;
        
        [Header("Events")]
        public UnityEvent<float> onImpact;
        
        private void OnCollisionEnter(Collision other)
        {
            float impactMagnitude = (other.impulse / Time.fixedDeltaTime).magnitude;
            if (impactMagnitude > ImpactThreshold)
                onImpact.Invoke(impactMagnitude);
            
            //Debug.Log("Impact magnitude: " + impactMagnitude);
        }
    }
}