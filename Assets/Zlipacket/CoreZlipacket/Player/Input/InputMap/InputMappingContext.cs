using UnityEngine;

namespace Zlipacket.CoreZlipacket.Player.Input.InputMap
{
    public abstract class InputMappingContext
    {
        protected InputSystem_Actions inputSystem;
        
        protected InputMappingContext(InputSystem_Actions inputSystem)
        {
            this.inputSystem = inputSystem;
        }
        
        public abstract void OnEnable();
        public abstract void OnDisable();
        
        public abstract void SetMapEnable(bool enable);
    }
}