using Items;
using Items.Script;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace Players.PlayerScripts
{
    public class PlayerInteractor : PlayerScript
    {
        private RaycastHit hit;
        public GameObject selectedObject { get; private set; }
        public Interactor interactor => selectedObject != null ? selectedObject.TryGetComponent(
            out Interactor interactor) ? interactor : null : null;
        
        public Vector3 hitPosition => hit.point;
        
        public bool allowChangeTarget = true;
        public bool isHovering;

        [Header("Config")]
        public Image crosshair;
        public LayerMask interactionLayer;
        public float minDistance = 0.1f;
        public float maxDistance = 3.5f;
        
        [Header("Event")]
        public UnityEvent onHover, onUnHover, onHovering;
        
        private void OnEnable()
        {
            playerInputMap.interactEvent += Interact;
        }

        private void OnDisable()
        {
            playerInputMap.interactEvent -= Interact;
        }

        private void Update()
        {
            if (!allowChangeTarget)
                return;
            
            if (TrySelect())
                Hover();
            else
                UnHover();
            
            Hovering();
        }
        
        public void Interact()
        {
            if (interactor != null)
                interactor.Interact(player);
        }

        private bool TrySelect()
        {
            if (Physics.Raycast(cam.transform.position + (cam.transform.forward * minDistance),
                    cam.transform.forward, out hit, maxDistance, interactionLayer))
            {
                if (hit.collider.gameObject.TryGetComponent<Interactor>(out var _))
                {
                    //Change the target.
                    /*if (isHovering)
                        UnHover(); //Just in case we already hovering something.*/
                    if (selectedObject == null)
                        selectedObject = hit.collider.gameObject;
                    else if (selectedObject != hit.collider.gameObject)
                    {
                        UnHover();
                        selectedObject = hit.collider.gameObject;
                    }
                    
                    return true;
                }
            }
            
            return false;
        }

        public void Hover()
        {
            if (isHovering)
                return;
            
            //Debug.Log("Hover");
            
            isHovering = true;
            CrosshairAnimation();
            
            selectedObject = hit.collider.gameObject;
            interactor?.onHover?.Invoke(player);
            onHover?.Invoke();
        }

        public void UnHover()
        {
            if (!isHovering)
                return;
            
            //Debug.Log("UnHover");

            isHovering = false;
            CrosshairAnimation(true);
            
            interactor?.onUnHover?.Invoke(player);
            onUnHover?.Invoke();
            selectedObject = null;
        }

        public void Hovering()
        {
            if (!isHovering)
                return;
            
            interactor?.onHovering?.Invoke(player);
            onHovering?.Invoke();
        }
        
        public void ResetInteractor()
        {
            allowChangeTarget = true;
            isHovering = false;
            selectedObject = null;
            CrosshairAnimation(true);
        }
        
        public void CrosshairAnimation(bool isReverse = false)
        {
            if (crosshair == null)
                return;
            
            float duration = 0.2f;
            if (!isReverse)
                crosshair.transform.DOScale(1.5f, duration);
            else
                crosshair.transform.DOScale(1f, duration);
        }
    }
}