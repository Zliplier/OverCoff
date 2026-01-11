using System;
using Interactable;
using Players;
using UnityEngine;
using UnityEngine.Events;
using Zlipacket.CoreZlipacket.Tools;

namespace Items.Script.Furniture
{
    public class Door : ItemScript
    {
        public HingeJoint door;
        [SerializeField] private Interactor interactor;

        [Header("Configs")]
        public bool onlyFrontOpen = false;
        [Range(0, 180)] public float openAngle = 100f;
        [SerializeField] private DoorState doorState = DoorState.Closed;
        
        [Header("Events")]
        public UnityEvent onOpen;
        public UnityEvent onClose;

        private void OnValidate()
        {
            switch (doorState)
            {
                case DoorState.Closed:
                    door.transform.localRotation = Quaternion.identity;
                    Close();
                    break;
                case DoorState.FrontOpen:
                    door.transform.localRotation = Quaternion.Euler(0f, -openAngle, 0f);
                    OpenFront();
                    break;
                case DoorState.BackOpen:
                    door.transform.localRotation = Quaternion.Euler(0f, onlyFrontOpen? -openAngle : openAngle, 0f);
                    OpenBack();
                    break;
            }
        }

        private void Start()
        {
            interactor.onInteract.AddListener(Interact);
        }
        
        private void Interact(Player player)
        {
            bool isFrontInteract = true;
            
            //Compare position to find if forward position is within Remap Vector of the door and player, if it is then 
            //player interact it from the front if not it's the back.
            float interactDirection = ZlipUtilities.RemapVector3Distance(transform.position + transform.forward, transform.position, player.transform.position);
            
            //              [Door]  <Front>     [Player]
            if (interactDirection is >= 0f and < 1f)
                isFrontInteract = true;
            
            //  [Player]    [Door]  <Front>
            else
                isFrontInteract = false;
            
            //Debug.Log("Open from front: " + isFrontInteract + " with the direction of: " + interactDirection);
            
            switch (doorState)
            {
                case DoorState.Closed:
                    if (isFrontInteract)
                        OpenFront();
                    else
                        OpenBack();
                    break;
                
                case DoorState.FrontOpen:
                case DoorState.BackOpen:
                    Close();
                    break;
            }
        }

        public void OpenFront()
        {
            doorState = DoorState.FrontOpen;
            
            JointSpring spring = door.spring;
            spring.targetPosition = -openAngle;
            door.spring = spring;
        }

        public void OpenBack()
        {
            doorState = DoorState.BackOpen;
            
            JointSpring spring = door.spring;
            spring.targetPosition = onlyFrontOpen? -openAngle : openAngle;
            door.spring = spring;
        }

        public void Close()
        {
            doorState = DoorState.Closed;
            
            JointSpring spring = door.spring;
            spring.targetPosition = 0f;
            door.spring = spring;
        }
    }

    public enum DoorState
    {
        FrontOpen, 
        BackOpen, 
        Closed
    }
}