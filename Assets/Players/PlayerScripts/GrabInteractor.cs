using System;
using System.Collections;
using Items;
using Items.Script;
using UnityEngine;
using UnityEngine.Events;
using Zlipacket.CoreZlipacket.Player.Input;
using Random = System.Random;

namespace Players.PlayerScripts
{
    [RequireComponent(typeof(PlayerInteractor))]
    public class GrabInteractor : PlayerScript
    {
        public GameObject holdArea;
        
        [SerializeField] private PlayerInteractor playerInteractor;
        
        private RaycastHit hit;
        private GameObject grabObject;
        private ItemGrab itemGrab =>
            grabObject != null ? grabObject.TryGetComponent<ItemGrab>(out var item) ? item : null : null;
        private Vector3 hitPosition => hit.point;
        
        [HideInInspector] public bool isGrabbing = false;
        
        [Header("Config")]
        public LayerMask interactionLayer;
        public float minDistance = 0.1f;
        public float maxDistance = 3.5f;
        public float minScrollDistance = 1f;
        public float maxScrollDistance => maxDistance;
        
        [Header("Event")]
        public UnityEvent onGrab, onDrop;
        
        private void OnEnable()
        {
            inputReader.leftMouseDownEvent += Grab;
            inputReader.leftMouseUpEvent += Drop;
        }

        private void OnDisable()
        {
            inputReader.leftMouseDownEvent -= Grab;
            inputReader.leftMouseUpEvent -= Drop;

            itemGrab?.Reset();
        }

        private void Grab()
        {
            if (!isGrabbing)
                StartGrab();
        }

        private void Drop()
        {
            if (isGrabbing)
                EndGrab();
        }

        private bool TryGrab()
        {
            if (Physics.Raycast(cam.transform.position + (cam.transform.forward * minDistance),
                    cam.transform.forward, out hit, maxDistance, interactionLayer))
            {
                if (hit.collider.gameObject.TryGetComponent<ItemGrab>(out var _))
                {
                    grabObject = hit.collider.gameObject;
                    return true;
                }
            }
            
            return false;
        }
        
        private void StartGrab()
        {
            if (!TryGrab())
                return;
            
            //Debug.Log("Starting Grab");
            
            isGrabbing = true;
            playerInteractor.allowChangeTarget = false;
            
            float range = (cam.transform.position - hitPosition).magnitude;
            holdArea.transform.position = (range < minScrollDistance) ?
                hitPosition + (cam.transform.forward * (minScrollDistance - range)) : hitPosition;

            Vector3 objectRotation = Vector3.ProjectOnPlane(grabObject.transform.forward, Vector3.up);
            if (objectRotation != Vector3.zero)
                holdArea.transform.rotation = Quaternion.LookRotation(objectRotation, Vector3.up);
            
            itemGrab?.Grab(this);
        }

        private void EndGrab()
        {
            //Debug.Log("Ending grab");
            
            playerInteractor.allowChangeTarget = true;
            isGrabbing = false;
            
            itemGrab?.Drop();
            
            grabObject = null;
        }

        public void Reset()
        {
            playerInteractor.allowChangeTarget = true;
            isGrabbing = false;
            grabObject = null;
        }
    }
}