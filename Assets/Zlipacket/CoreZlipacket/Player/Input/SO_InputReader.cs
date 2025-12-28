using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Zlipacket.CoreZlipacket.Player.Input
{
    [CreateAssetMenu(menuName = "Zlipacket/Player/Input Reader", fileName = "Input Reader")]
    public class SO_InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions, InputSystem_Actions.IUIActions
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
        
        public event UnityAction<bool> slot1;
        public event UnityAction<bool> slot2;
        public event UnityAction<bool> slot3;
        
        public event UnityAction<bool> escapeEvent;
        
        public event UnityAction<Vector2> mouseMoveEvent;

        private void OnEnable()
        {
            if (inputSystem == null)
            {
                inputSystem = new InputSystem_Actions();
            }
            
            SetPlayerInputEnable(true);
            
            inputSystem.Player.Move.performed += OnMove;
            inputSystem.Player.Move.canceled += OnMove;
            inputSystem.Player.Sprint.performed += OnSprint;
            inputSystem.Player.Sprint.canceled += OnSprint;
            inputSystem.Player.Jump.started += OnJump;
            inputSystem.Player.Look.performed += OnLook;
            inputSystem.Player.Interact.started += OnInteract;
            
            inputSystem.Player.LeftMouse.started += OnLeftMouse;
            inputSystem.Player.LeftMouse.canceled += OnLeftMouse;
            inputSystem.Player.MouseWheel.performed += OnMouseWheel;
            
            inputSystem.Player.Slot1.started += OnSlot1;
            inputSystem.Player.Slot2.started += OnSlot2;
            inputSystem.Player.Slot3.started += OnSlot3;
        }

        private void OnDisable()
        {
            if (inputSystem == null)
                return;
            
            SetPlayerInputEnable(false);
            
            inputSystem.Player.Move.performed -= OnMove;
            inputSystem.Player.Move.canceled -= OnMove;
            inputSystem.Player.Sprint.performed -= OnSprint;
            inputSystem.Player.Sprint.canceled -= OnSprint;
            inputSystem.Player.Jump.started -= OnJump;
            inputSystem.Player.Look.performed -= OnLook;
            inputSystem.Player.Interact.started -= OnInteract;
            
            inputSystem.Player.LeftMouse.started -= OnLeftMouse;
            inputSystem.Player.LeftMouse.canceled -= OnLeftMouse;
            inputSystem.Player.MouseWheel.performed -= OnMouseWheel;
            
            inputSystem.Player.Slot1.started -= OnSlot1;
            inputSystem.Player.Slot2.started -= OnSlot2;
            inputSystem.Player.Slot3.started -= OnSlot3;
        }
        
        #region Player Actions
        public void SetPlayerInputEnable(bool enable)
        {
            if (enable)
                inputSystem.Player.Enable();
            else
                inputSystem.Player.Disable();
        }

        public void SetUiInputEnable(bool enable)
        {
            if (enable)
                inputSystem.UI.Enable();
            else
                inputSystem.UI.Disable();
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
            if (context.phase == InputActionPhase.Started)
                leftMouseDownEvent?.Invoke();
            if (context.phase == InputActionPhase.Canceled)
                leftMouseUpEvent?.Invoke();
        }
        
        public void OnRightMouse(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                rightMouseDownEvent?.Invoke();
            if (context.phase == InputActionPhase.Canceled)
                rightMouseUpEvent?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                interactEvent?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            crouchEvent?.Invoke(context.ReadValueAsButton());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
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
            if (context.phase == InputActionPhase.Performed)
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
            if (context.phase == InputActionPhase.Started)
                returnEvent?.Invoke(context.ReadValueAsButton());
        }

        public void OnSlot1(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                slot1?.Invoke(context.ReadValueAsButton());
        }

        public void OnSlot2(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                slot2?.Invoke(context.ReadValueAsButton());
        }

        public void OnSlot3(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                slot3?.Invoke(context.ReadValueAsButton());
        }

        public void OnEscape(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                escapeEvent?.Invoke(context.ReadValueAsButton());
        }

        #endregion
        
        #region UI Actions
        public void OnNavigate(InputAction.CallbackContext context)
        {
            
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            
        }
        #endregion
    }
}