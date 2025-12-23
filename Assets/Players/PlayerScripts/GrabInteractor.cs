using System;
using System.Collections;
using Items;
using Items.ItemScript;
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
        private GameObject grabObject => hit.collider.gameObject;

        private ItemGrab itemGrab =>
            grabObject != null ? grabObject.TryGetComponent<ItemGrab>(out var item) ? item : null : null;
        
        [HideInInspector] public bool isGrabbing = false;
        
        [Header("Config")]
        public float minDistance = 1.5f;
        public float maxDistance = 3.5f;
        
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
        }

        private void Grab()
        {
            if (!isGrabbing)
            {
                StartGrab();
            }
        }

        private void Drop()
        {
            if (isGrabbing)
                EndGrab();
        }

        private bool TryGrab()
        {
            if (Physics.Raycast(cam.transform.position + (cam.transform.forward * minDistance),
                    cam.transform.forward, out hit, maxDistance))
            {
                if (itemGrab != null)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        private void StartGrab()
        {
            if (!TryGrab())
                return;
            
            isGrabbing = true;
            playerInteractor.allowChangeTarget = false;
            
            holdArea.transform.position = hit.point;
            holdArea.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(
                grabObject.transform.forward, Vector3.up), Vector3.up);
            
            itemGrab?.Grab(this);
        }

        private void EndGrab()
        {
            Reset();
            
            isGrabbing = false;
            itemGrab?.Drop();
        }

        public void Reset()
        {
            playerInteractor.allowChangeTarget = true;
            isGrabbing = false;
        }
    }
}