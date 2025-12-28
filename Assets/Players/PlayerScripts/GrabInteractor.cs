using System;
using System.Collections;
using Items;
using Items.Script;
using UnityEngine;
using UnityEngine.Events;
using Zlipacket.CoreZlipacket.Player.Input;
using Zlipacket.CoreZlipacket.Tools;
using Random = System.Random;

namespace Players.PlayerScripts
{
    public class GrabInteractor : PlayerScript
    {
        public GameObject holdArea;
        
        [SerializeField] private PlayerInteractor playerInteractor;
        
        private RaycastHit hit;
        private Vector3 hitPosition => hit.point;
        
        private GameObject grabObject;
        private ItemGrab itemGrab;
        private Rigidbody rbGrab;
        
        public bool isGrabbing => grabObject != null;
        
        [Header("Config")]
        public LayerMask interactionLayer;
        public float minDistance = 0.1f;
        public float maxDistance = 3.5f;
        public float minScrollDistance = 1f;
        public float maxScrollDistance => maxDistance;
        public float scrollSpeed = 0.5f;
        private float minScroll => itemGrab.minScrollOverride? itemGrab.overrideMinScroll + minScrollDistance: minScrollDistance;
        private float maxScroll => itemGrab.maxScrollOverride? itemGrab.overrideMaxScroll + maxScrollDistance: maxScrollDistance;
        
        [Space]
        public float grabForce = 8f;
        public float rotationForce = 10f;
        
        [Header("Event")]
        public UnityEvent onGrab, onDrop;
        
        private void OnEnable()
        {
            inputReader.leftMouseDownEvent += Grab;
            inputReader.leftMouseUpEvent += Drop;
            inputReader.mouseScrollEvent += MouseScroll;
        }

        private void OnDisable()
        {
            inputReader.leftMouseDownEvent -= Grab;
            inputReader.leftMouseUpEvent -= Drop;
            inputReader.mouseScrollEvent -= MouseScroll;

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

        private void AssignGrabObject(GameObject grabObject)
        {
            this.grabObject = grabObject;
            itemGrab = grabObject.GetComponent<ItemGrab>();
            rbGrab = grabObject.GetComponent<Rigidbody>();
        }

        private void NullGrabObject()
        {
            grabObject = null;
            itemGrab = null;
            rbGrab = null;
        }
        
        private bool TryGrab()
        {
            if (Physics.Raycast(cam.transform.position + (cam.transform.forward * minDistance),
                    cam.transform.forward, out hit, maxDistance, interactionLayer))
            {
                if (hit.collider.gameObject.TryGetComponent<ItemGrab>(out var _))
                {
                    AssignGrabObject(hit.collider.gameObject);
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
            
            rbGrab.useGravity = false;
            rbGrab.freezeRotation = true;
            rbGrab.linearVelocity = Vector3.zero;
            
            if (playerInteractor != null)
                playerInteractor.allowChangeTarget = false;

            SetHoldAreaPosition();
            SetHoldAreaRotation();
            
            itemGrab.Grab(this);
        }

        private void SetHoldAreaPosition()
        {
            if (!itemGrab.grabPositionOverride)
            {
                float range = (cam.transform.position - hitPosition).magnitude;
                //Compare if hitPosition is in minimum range or not 
                holdArea.transform.position = (range < minScroll) ?
                    //I actually dont know what the line below does. It just work.
                    hitPosition + (cam.transform.forward * (minScroll - range)) : hitPosition;
            }
            else
            {
                //Overriding Position relative to local minScroll and camera direction.
                //Get minScroll in world position.
                Vector3 holdAreaPos = cam.transform.position + (cam.transform.forward * minScroll);
                //Rotate config position to make it relative to minScroll and add them.
                holdAreaPos += cam.transform.rotation * itemGrab.overridePosition;
                holdArea.transform.position = holdAreaPos;
            }
        }

        private void SetHoldAreaRotation()
        {
            if (!itemGrab.grabRotationOverride)
            {
                //Preserve Object Rotation
                Vector3 objectRotation = Vector3.ProjectOnPlane(grabObject.transform.forward, Vector3.up);
                if (objectRotation != Vector3.zero)
                    holdArea.transform.rotation = Quaternion.LookRotation(objectRotation, Vector3.up);
            }
            else
            {
                //Overriding Rotation relative to camera direction.
                holdArea.transform.rotation = cam.transform.rotation * itemGrab.overrideRotation;
            }
        }
        
        private void EndGrab()
        {
            //Debug.Log("Ending grab");
            
            if (playerInteractor != null)
                playerInteractor.allowChangeTarget = true;
            
            rbGrab.useGravity = true;
            rbGrab.freezeRotation = false;
            itemGrab.Drop();
            
            NullGrabObject();
        }

        public void ResetGrab()
        {
            if (playerInteractor != null)
                playerInteractor.ResetInteractor();
            NullGrabObject();
        }

        /*private void Update()
        {
            if (isGrabbing)
                UpdatePosition();
        }*/

        private void FixedUpdate()
        {
            if (isGrabbing)
            {
                UpdatePosition();
                UpdateRotation();
            }
        }
        
        private void UpdatePosition()
        {
            Vector3 direction = holdArea.transform.position - grabObject.transform.position;
            rbGrab.linearVelocity = direction * (
                (itemGrab.grabForceOverride? itemGrab.overrideGrabForce: grabForce) 
                * (1 + Time.deltaTime));
        }
        
        private void UpdateRotation()
        {
            grabObject.transform.rotation = Quaternion.RotateTowards(grabObject.transform.rotation, holdArea.transform.rotation, 
                rotationForce * (1 + Time.deltaTime));
        }
        
        public void MouseScroll(float scrollValue)
        {
            if (!isGrabbing)
                return;
            if (itemGrab.scrollDisable)
                return;
            
            //Try to find direction parallel to camera if position is overrode.
            Vector3 nearPosition = cam.transform.position + (cam.transform.forward * minScroll);
            Vector3 farPosition = cam.transform.position + (cam.transform.forward * maxScroll);
            if (itemGrab.grabPositionOverride)
            {
                nearPosition += cam.transform.rotation * itemGrab.overridePosition;
                farPosition += cam.transform.rotation * itemGrab.overridePosition;
            }
            Vector3 holdAreaDirection = cam.transform.forward;
            //Vector3.Normalize(holdAreaDirection);
            
            //Get the Extrapolation of the holdArea if it is within range of near and far.
            float remap = ZlipUtilities.RemapVector3Distance(
                holdArea.transform.position + holdAreaDirection * 
                (scrollValue > 0 ? scrollSpeed : -scrollSpeed)
                , nearPosition, farPosition);
            
            if (scrollValue > 0) //Scroll Forward
            {
                if (remap < 1f)
                {
                    holdArea.transform.position += holdAreaDirection * scrollSpeed;
                }
                else
                {
                    holdArea.transform.position = farPosition;
                }
            }
            else if (scrollValue < 0) //Scroll Backward
            {
                if (remap > 0f)
                {
                    holdArea.transform.position += holdAreaDirection * -scrollSpeed;
                }
                else
                {
                    holdArea.transform.position = nearPosition;
                }
            }
        }
    }
}