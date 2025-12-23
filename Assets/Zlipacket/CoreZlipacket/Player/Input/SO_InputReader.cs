using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Zlipacket.CoreZlipacket.Player.Input
{
    [CreateAssetMenu(menuName = "Zlipacket/Player/Input Reader", fileName = "Input Reader")]
    public class SO_InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions
    {
        private InputSystem_Actions inputSystem;

        public event UnityAction<Vector2> movementEvent;
        public event UnityAction<bool> sprintEvent;
        public event UnityAction jumpEvent;
        public event UnityAction<Vector2> lookEvent;

        public event UnityAction leftMouseDownEvent;
        public event UnityAction leftMouseUpEvent;
        public event UnityAction rightMouseDownEvent;
        public event UnityAction rightMouseUpEvent;
        public event UnityAction<float> mouseScrollEvent;

        public event UnityAction interactEvent;
        public event UnityAction<bool> throwEvent;
        public event UnityAction<bool> crouchEvent;
        public event UnityAction<bool> returnEvent;
        
        public event UnityAction<bool> escapeEvent;
        
        public event UnityAction<Vector2> mouseMoveEvent;

        private void OnEnable()
        {
            if (inputSystem == null)
            {
                inputSystem = new InputSystem_Actions();
            }
            
            inputSystem.Player.Enable();
            
            inputSystem.Player.Move.performed += OnMove;
            inputSystem.Player.Move.canceled += OnMove;
            inputSystem.Player.Sprint.performed += OnSprint;
            inputSystem.Player.Sprint.canceled += OnSprint;
            inputSystem.Player.Jump.started += OnJump;
            inputSystem.Player.Look.performed += OnLook;
            inputSystem.Player.Interact.started += OnInteract;
            inputSystem.Player.LeftMouse.started += OnLeftMouse;
            inputSystem.Player.LeftMouse.canceled += OnLeftMouse;
        }

        private void OnDisable()
        {
            if (inputSystem == null)
                return;
            
            inputSystem.Player.Disable();
            
            inputSystem.Player.Move.performed -= OnMove;
            inputSystem.Player.Move.canceled -= OnMove;
            inputSystem.Player.Sprint.performed -= OnSprint;
            inputSystem.Player.Sprint.canceled -= OnSprint;
            inputSystem.Player.Jump.started -= OnJump;
            inputSystem.Player.Look.performed -= OnLook;
            inputSystem.Player.Interact.started -= OnInteract;
            inputSystem.Player.LeftMouse.started -= OnLeftMouse;
            inputSystem.Player.LeftMouse.canceled -= OnLeftMouse;
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            movementEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            lookEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLeftMouse(InputAction.CallbackContext context)
        {
            if (context.started)
                leftMouseDownEvent?.Invoke();
            if (context.canceled)
                leftMouseUpEvent?.Invoke();
        }
        
        public void OnRightMouse(InputAction.CallbackContext context)
        {
            if (context.started)
                rightMouseDownEvent?.Invoke();
            if (context.canceled)
                rightMouseUpEvent?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            interactEvent?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            crouchEvent?.Invoke(context.ReadValueAsButton());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            jumpEvent?.Invoke();
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            sprintEvent?.Invoke(context.ReadValueAsButton());
        }

        public void OnMouseWheel(InputAction.CallbackContext context)
        {
            mouseScrollEvent?.Invoke(context.ReadValue<float>());
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            throwEvent?.Invoke(context.ReadValueAsButton());
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            mouseMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnReturn(InputAction.CallbackContext context)
        {
            returnEvent?.Invoke(context.ReadValueAsButton());
        }
    }
}