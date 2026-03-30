using AI.FSM;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Customer.CustomerState
{
    public class CustomerDone : CustomerState
    {
        public CustomerDone(E_CustomerState key, CustomerController customerController) : base(key, customerController)
        {
            
        }

        public override void EnterState()
        {
            Debug.Log("Entering Done State");
            customerController.DestroyOnDone();
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }

        public override void FixedUpdateState()
        {
            
        }

        public override void ExitState()
        {
            
        }
    }
}