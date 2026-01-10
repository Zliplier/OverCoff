using UnityEngine;
using UnityEngine.Events;

namespace Zlipacket.CoreZlipacket.Tools.EventDispatcher
{
    [RequireComponent(typeof(Collider))]
    public class TriggerEventDispatcher : MonoBehaviour
    {
        [Header("Events")]
        private UnityEvent<Collider> onTriggerEnter;
        private UnityEvent<Collider> onTriggerExit;
        private UnityEvent<Collider> onTriggerStay;

        private void OnTriggerEnter(Collider other) 
            => onTriggerEnter.Invoke(other);

        private void OnTriggerExit(Collider other) 
            => onTriggerExit.Invoke(other);

        private void OnTriggerStay(Collider other) 
            => onTriggerStay.Invoke(other);
    }
}