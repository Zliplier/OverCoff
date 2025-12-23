using System;
using Items;
using Items.ItemScript;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using Zlipacket.CoreZlipacket.Player.Input;

namespace Players.PlayerScripts
{
    public class PlayerInteractor : PlayerScript
    {
        private RaycastHit raycastHit;
        public GameObject selectedObject => raycastHit.collider.gameObject;
        private Interactor interactor => selectedObject != null ? selectedObject.TryGetComponent(
            out Interactor interactor) ? interactor : null : null;
        public Vector3 hitPosition => raycastHit.point;
        
        public bool allowChangeTarget = true;
        public bool isHovering;
        
        [Header("Config")]
        public LayerMask interactionLayer;
        public float minDistance = 1.5f;
        public float maxDistance = 3.5f;
        
        [Header("Event")]
        public UnityEvent onHover, onUnHover;
        
        private void OnEnable()
        {
            inputReader.interactEvent += Interact;
        }

        private void OnDisable()
        {
            inputReader.interactEvent -= Interact;
        }

        private void Update()
        {
            if (!allowChangeTarget)
                return;
            
            if (Physics.Raycast(cam.transform.position + (cam.transform.forward * minDistance),
                    cam.transform.forward, out raycastHit, maxDistance, interactionLayer))
            {
                Hover();
            }
            else
            {
                UnHover();
            }
        }
        
        public void Interact()
        {
            if (selectedObject == null)
                return;
            
            if (selectedObject.TryGetComponent(out ItemInteractor itemInteractor))
            {
                itemInteractor.Interact();
            }
        }

        public void Hover()
        {
            if (!isHovering)
            {
                isHovering = true;
                
                interactor?.onHover?.Invoke();
                onHover?.Invoke();
            }
        }

        public void UnHover()
        {
            if (isHovering)
            {
                isHovering = false;
                
                interactor?.onUnHover?.Invoke();
                onUnHover?.Invoke();
            }
        }
    }
}