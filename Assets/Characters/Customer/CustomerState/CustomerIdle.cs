using AI.FSM;
using Characters.Customer.Waypoint;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Customer.CustomerState
{
    public class CustomerIdle : CustomerState
    {
        private float idleTime = 0f;
        private float timeElapsed;
        
        public CustomerIdle(E_CustomerState key, CustomerController customerController) : base(key, customerController)
        {
            
        }

        public override void EnterState()
        {
            Debug.Log("Entering Idle State");
            nextState = E_CustomerState.Walk;
            stateCheck = () =>
            {
                if (timeElapsed >= idleTime)
                {
                    customerController.chair = ChairManager.Instance.RequestChair();
                    if (customerController.chair != null)
                    {
                        customerController.chair.occupant = customerManager.gameObject;
                        return true;
                    }
                    else
                    {
                        ResetState();
                    }
                }
                
                return false;
            };
            ResetState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            
            timeElapsed += Time.deltaTime;
        }

        public override void FixedUpdateState()
        {
            
        }

        public override void ExitState()
        {
            timeElapsed = 0;
            customerController.MoveTo(customerController.chair.transform);
        }

        private void ResetState()
        {
            idleTime = Random.Range(1.5f, 3f);
            timeElapsed = 0f;
        }
    }
}