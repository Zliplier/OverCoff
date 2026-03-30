using AI.FSM;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Customer.CustomerState
{
    public class CustomerWalk : CustomerState
    {
        public CustomerWalk(E_CustomerState key, CustomerController customerController) : base(key, customerController)
        {
            
        }

        public override void EnterState()
        {
            Debug.Log("Entering Walk State");
            customerController.animator.SetTrigger("Walk");
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