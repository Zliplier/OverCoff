using AI.FSM;
using Characters.Customer.CustomerState;
using Characters.Customer.Waypoint;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Customer
{
    [RequireComponent(typeof(NavMeshAgent),  typeof(Rigidbody), typeof(Collider))]
    public class CustomerController : StateManager<E_CustomerState>
    {
        public NavMeshAgent agent;
        public Chair chair;
        public Animator animator;
        public float sitOffset = 0;
        public GameObject mesh;
        [HideInInspector] public Vector3 spawnpoint;
        [HideInInspector] public Vector3 unsitPoint;

        [HideInInspector] public bool orderDelivered = false;
        [HideInInspector] public CustomerOrdering customerOrdering;
        
        protected override void Start()
        {
            spawnpoint = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            
            agent = GetComponent<NavMeshAgent>();
            customerOrdering = GetComponent<CustomerOrdering>();
            
            AddState(new CustomerIdle(E_CustomerState.Idle, this));
            AddState(new CustomerWalk(E_CustomerState.Walk, this));
            AddState(new CustomerSit(E_CustomerState.Sit, this));
            AddState(new CustomerOrder(E_CustomerState.Order, this));
            AddState(new CustomerUnsit(E_CustomerState.Unsit, this));
            AddState(new CustomerDone(E_CustomerState.Done, this));
            
            CurrentState = States[E_CustomerState.Idle];
            States[E_CustomerState.Walk].SetTransitionToState(E_CustomerState.Sit, () =>
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    return true;
                }
                return false;
            });
            States[E_CustomerState.Sit].SetTransitionToState(E_CustomerState.Order, () =>
            {
                return true;
            });
            States[E_CustomerState.Order].SetTransitionToState(E_CustomerState.Unsit, () =>
            {
                return orderDelivered;
            });
            States[E_CustomerState.Unsit].SetTransitionToState(E_CustomerState.Walk, () =>
            {
                return true;
            });
            
            base.Start();
        }

        public void MoveTo(Transform transform)
        {
            agent.isStopped = false;
            agent.SetDestination(transform.position);
        }
        
        public void MoveTo(Vector3 position)
        {
            agent.isStopped = false;
            agent.SetDestination(position);
        }

        public void Stay()
        {
            agent.isStopped = true;
        }

        public void OrderDelivered()
        {
            orderDelivered = true;
        }

        public void DestroyOnDone()
        {
            Destroy(gameObject);
        }
    }
}
