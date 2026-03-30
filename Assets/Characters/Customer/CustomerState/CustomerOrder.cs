using AI.FSM;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Customer.CustomerState
{
    public class CustomerOrder : CustomerState
    {
        public CustomerOrder(E_CustomerState key, CustomerController customerController) : base(key, customerController)
        {
            
        }

        public override void EnterState()
        {
            Debug.Log("Entering customer order");
            
            customerController.customerOrdering.StartOrder();
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