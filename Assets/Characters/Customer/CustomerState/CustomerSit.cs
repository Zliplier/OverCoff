using AI.FSM;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Customer.CustomerState
{
    public class CustomerSit : CustomerState
    {
        public CustomerSit(E_CustomerState key, CustomerController customerController) : base(key, customerController)
        {
            
        }

        public override void EnterState()
        {
            Debug.Log("Entering customer sit state");
            customerController.animator.SetTrigger("Sit");
            customerController.Stay();
            
            customerController.unsitPoint = new Vector3(
                customerController.transform.position.x,
                customerController.transform.position.y, 
                customerController.transform.position.z);

            customerController.transform.position = customerController.chair.transform.position;
            customerController.mesh.transform.position = new Vector3(
                customerController.mesh.transform.position.x,
                customerController.mesh.transform.position.y + customerController.sitOffset,
                customerController.mesh.transform.position.z);
            
            customerController.transform.rotation = Quaternion.LookRotation(customerController.chair.transform.forward, 
                customerController.chair.transform.up);
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