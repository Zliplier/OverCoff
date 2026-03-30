using AI.FSM;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Customer.CustomerState
{
    public class CustomerUnsit : CustomerState
    {
        public CustomerUnsit(E_CustomerState key, CustomerController customerController) : base(key, customerController)
        {
            
        }

        public override void EnterState()
        {
            Debug.Log("Entering customer unsit");
            customerController.transform.position = customerController.unsitPoint;
            customerController.mesh.transform.position = new Vector3(
                customerController.mesh.transform.position.x,
                0,
                customerController.mesh.transform.position.z);
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
            customerController.MoveTo(customerController.spawnpoint);
            customerController.States[E_CustomerState.Walk].SetTransitionToState(E_CustomerState.Done);
        }
    }
}