using System.Collections;
using Players.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace Items.ItemScript
{
    [RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Item))]
    public class ItemGrab : MonoBehaviour
    {
        private Item item;
        private Rigidbody rb => item.rb;
        
        private GameObject holdArea;
        private Transform holdTransform => holdArea.transform;
        private GrabInteractor holder = null;
        
        private Coroutine co_Grabbing = null;
        public bool isGrabbing => co_Grabbing != null;

        [Header("Config")]
        public float grabForce = 8f;
        
        [Header("Event")]
        public UnityEvent onGrab, onDrop;
        
        public void Grab(GrabInteractor holder)
        {
            Debug.Log($"Start Grab");
            if (isGrabbing)
                StopCoroutine(co_Grabbing);

            if (holder != null)
                holder.Reset();
            
            this.holder = holder;
            holdArea = holder.holdArea;
            co_Grabbing = StartCoroutine(Grabbing());
            
            onGrab?.Invoke();
        }

        public void Drop()
        {
            if (isGrabbing)
                StopCoroutine(co_Grabbing);
            
            co_Grabbing = null;
            holdArea = null;

            holder.isGrabbing = false;
            holder = null;
            
            onDrop?.Invoke();
        }

        private IEnumerator Grabbing()
        {
            while (isGrabbing)
            {
                UpdatePosition();
                UpdateRotation();
                
                yield return new WaitForFixedUpdate();
            }
        }

        private void UpdatePosition()
        {
            Vector3 direction = holdTransform.position - transform.position;
            rb.linearVelocity = direction * (grabForce * (1 + Time.deltaTime));
        }

        private void UpdateRotation()
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, holdTransform.rotation, 
                10f * (1 + Time.deltaTime));
        }
    }
}