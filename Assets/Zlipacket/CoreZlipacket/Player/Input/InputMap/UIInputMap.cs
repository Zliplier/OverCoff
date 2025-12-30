using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using NotImplementedException = System.NotImplementedException;

namespace Zlipacket.CoreZlipacket.Player.Input.InputMap
{
    public class UIInputMap : InputMappingContext, InputSystem_Actions.IUIActions
    {
        public Vector2 mousePosition;

        public event UnityAction<bool> submitEvent;
        public event UnityAction<bool> cancelEvent;
        public event UnityAction<bool> inventoryEvent;
        
        public UIInputMap(InputSystem_Actions inputSystem) : base(inputSystem)
        {
            
        }

        public override void OnEnable()
        {
            SetMapEnable(true);
            
            inputSystem.UI.Submit.started += OnSubmit;
            inputSystem.UI.Cancel.started += OnCancel;
            inputSystem.UI.Inventory.started += OnInventory;
        }

        public override void OnDisable()
        {
            SetMapEnable(false);
            
            inputSystem.UI.Submit.started -= OnSubmit;
            inputSystem.UI.Cancel.started -= OnCancel;
            inputSystem.UI.Inventory.started -= OnInventory;
        }
        
        public override void SetMapEnable(bool enable)
        {
            if (enable)
                inputSystem.UI.Enable();
            else
                inputSystem.UI.Disable();
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                submitEvent?.Invoke(context.ReadValueAsButton());
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                cancelEvent?.Invoke(context.ReadValueAsButton());
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            mousePosition = context.ReadValue<Vector2>();
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

        public void OnInventory(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                inventoryEvent?.Invoke(context.ReadValueAsButton());
        }
    }
}