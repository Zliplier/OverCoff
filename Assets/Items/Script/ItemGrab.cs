using System;
using System.Collections;
using Players.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace Items.Script
{
    [RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Item))]
    public class ItemGrab : ItemScript
    {
        private GameObject holdArea;
        private Transform holdTransform => holdArea.transform;
        private GrabInteractor holder = null;
        
        private Coroutine co_Grabbing = null;
        public bool isGrabbing => co_Grabbing != null;

        [Header("Config")]
        public float grabForce = 8f;
        public float rotationForce = 10f;
        
        [Header("Event")]
        public UnityEvent onGrab, onDrop;

        private void OnDisable()
        {
            holder?.Reset();
        }

        public void Grab(GrabInteractor holder)
        {
            if (isGrabbing)
                StopCoroutine(co_Grabbing);

            if (holder != this.holder)
                this.holder?.Reset();
            
            this.holder = holder;
            holdArea = holder.holdArea;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.freezeRotation = true;
            co_Grabbing = StartCoroutine(Grabbing());
            
            onGrab?.Invoke();
        }

        public void Drop()
        {
            Reset();
            
            onDrop?.Invoke();
        }

        private IEnumerator Grabbing()
        {
            while (holdArea != null)
            {
                yield return new WaitForFixedUpdate();
                
                UpdatePosition();
                UpdateRotation();
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
                rotationForce * (1 + Time.deltaTime));
        }

        public void Reset()
        {
            if (isGrabbing)
                StopCoroutine(co_Grabbing);
            
            rb.useGravity = true;
            rb.freezeRotation = false;
            
            co_Grabbing = null;
            holdArea = null;
            
            holder.isGrabbing = false;
            holder = null;
        }
    }
}