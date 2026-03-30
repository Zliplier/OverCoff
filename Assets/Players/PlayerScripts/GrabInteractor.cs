using System;
using System.Collections;
using Items;
using Items.Script;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
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
        
        public GameObject grabObject { get; private set; }
        public ItemGrab itemGrab { get; private set; }
        private Rigidbody rbGrab;
        
        public bool isGrabbing => grabObject != null;
        
        [Header("Config")]
        public LayerMask interactionLayer;
        public float maxGrabHoldDistance = 3f;
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
            playerInputMap.rightMouseDownEvent += Grab;
            playerInputMap.rightMouseUpEvent += Drop;
            playerInputMap.mouseScrollEvent += MouseScroll;
            playerInputMap.leftMouseDownEvent += StartHoldingThrow;
            playerInputMap.leftMouseUpEvent += Throw;
        }

        private void OnDisable()
        {
            playerInputMap.rightMouseDownEvent -= Grab;
            playerInputMap.rightMouseUpEvent -= Drop;
            playerInputMap.mouseScrollEvent -= MouseScroll;
            playerInputMap.leftMouseDownEvent -= StartHoldingThrow;
            playerInputMap.leftMouseUpEvent -= Throw;

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
            
            if (playerInteractor != null && itemGrab.blockInteractor)
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
            
            if (itemGrab.useGravityAfterDrop)
                rbGrab.useGravity = true;
            rbGrab.freezeRotation = false;
            itemGrab.Drop();
            
            NullGrabObject();
        }

        public void ResetGrab()
        {
            if (playerInteractor != null)
                playerInteractor.ResetInteractor();
            
            if (itemGrab.useGravityAfterDrop)
                rbGrab.useGravity = true;
            rbGrab.freezeRotation = false;
            
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
                if (Vector3.Distance(grabObject.transform.position, cam.transform.position) > maxGrabHoldDistance)
                {
                    Drop();
                    return;
                }
                
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
            //Have no idea how rotation work, but whatever.
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
                //farPosition += cam.transform.rotation * itemGrab.overridePosition;
            }
            Vector3 holdAreaDirection = farPosition - nearPosition;
            holdAreaDirection = Vector3.Normalize(holdAreaDirection);
            
            //Get the Extrapolation of the holdArea if it is within range of near and far.
            float remap = ZlipUtilities.RemapVector3Distance(
                holdArea.transform.position + holdAreaDirection * 
                (scrollValue > 0 ? scrollSpeed : -scrollSpeed)
                , nearPosition, farPosition);
            
            if (scrollValue > 0) //Scroll Forward
            {
                if (remap < 1f)
                    holdArea.transform.position += holdAreaDirection * scrollSpeed;
                else
                    holdArea.transform.position = farPosition;
            }
            else if (scrollValue < 0) //Scroll Backward
            {
                if (remap > 0f)
                    holdArea.transform.position += holdAreaDirection * -scrollSpeed;
                else
                    holdArea.transform.position = nearPosition;
            }
        }

        [Header("Throw")]
        public Slider throwSlider;
        
        public Coroutine co_Throwing = null;
        public bool isHoldingThrow => co_Throwing != null;
        
        [Range(0, 1)] private float throwPower = 0f; //Percentage for throwForce.
        public float throwForce = 10f;
        public float timeToCharge = 3f;
        
        public void StartHoldingThrow()
        {
            if (!isGrabbing)
                return;
            
            if (isHoldingThrow)
            {
                StopCoroutine(co_Throwing);
                co_Throwing = null;
            }

            co_Throwing = StartCoroutine(HoldingThrow());
            
            //Debug.Log("Starting holding throw");
        }
        
        private IEnumerator HoldingThrow()
        {
            throwSlider.gameObject.SetActive(true);
            playerInteractor.allowChangeTarget = false;
            
            while (true)
            {
                if (throwPower < 1)
                    throwPower += Time.deltaTime / timeToCharge;
                else
                {
                    throwPower = 1f;
                }
                throwSlider.value = throwPower;

                //Debug.Log("Holding throw, Throw Power: " + throwPower);
                yield return new WaitForEndOfFrame();
            }
        }
        
        private void Throw()
        {
            if (!isHoldingThrow)
                return;
            
            throwSlider.gameObject.SetActive(false);
            throwSlider.value = 0f;
            playerInteractor.allowChangeTarget = true;
            
            StopCoroutine(co_Throwing);
            co_Throwing = null;
            
            //Debug.Log("Throwing throw");
            //Apply Force
            if (rbGrab == null)
                return;

            Rigidbody rbThrow = rbGrab;
            
            EndGrab();
            
            rbThrow.AddForce(cam.transform.forward * throwForce * throwPower, ForceMode.Impulse);

            throwPower = 0f;
        }
    }
}