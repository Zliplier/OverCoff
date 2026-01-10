using UnityEngine;
using UnityEngine.Events;

namespace Zlipacket.CoreZlipacket.Tools.EventDispatcher
{
    [RequireComponent(typeof(Collider))]
    public class CollisionEventDispatcher : MonoBehaviour
    {
        [Header("Events")]
        private UnityEvent<Collision> onCollisionEnter;
        private UnityEvent<Collision> onCollisionExit;
        private UnityEvent<Collision> onCollisionStay;

        private void OnCollisionEnter(Collision other)
            => onCollisionEnter?.Invoke(other);

        private void OnCollisionExit(Collision other)
            => onCollisionExit?.Invoke(other);

        private void OnCollisionStay(Collision other)
            => onCollisionEnter?.Invoke(other);
    }
}