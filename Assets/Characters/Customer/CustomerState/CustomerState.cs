using AI.FSM;

namespace Characters.Customer.CustomerState
{
    public abstract class CustomerState : BaseState<E_CustomerState, StateManager<E_CustomerState>>
    {
        public CustomerController customerController;
        
        public CustomerState(E_CustomerState key, CustomerController customerController) : base(key, customerController)
        {
            this.customerController = customerController;
        }
    }
}