using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using NotImplementedException = System.NotImplementedException;

namespace Zlipacket.CoreZlipacket.Player.Input.InputMap
{
    public class PlayerInputMap : InputMappingContext, InputSystem_Actions.IPlayerActions
    {
        public Vector2 mousePosition;
        
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
        
        public event UnityAction<bool> inventoryEvent;
        public event UnityAction<bool> escapeEvent;
        
        public PlayerInputMap(InputSystem_Actions inputSystem) : base(inputSystem)
        {
            
        }

        public override void OnEnable()
        {
            SetMapEnable(true);
            
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
            
            inputSystem.Player.Inventory.started += OnInventory;
        }

        public override void OnDisable()
        {
            SetMapEnable(false);
            
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
            
            inputSystem.Player.Inventory.started -= OnInventory;
        }
        
        public override void SetMapEnable(bool enable)
        {
            if (enable)
                inputSystem.Player.Enable();
            else
                inputSystem.Player.Disable();
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

        public void OnPoint(InputAction.CallbackContext context)
        {
            mousePosition = context.ReadValue<Vector2>();
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

        public void OnInventory(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                inventoryEvent?.Invoke(context.ReadValueAsButton());
        }
    }
}