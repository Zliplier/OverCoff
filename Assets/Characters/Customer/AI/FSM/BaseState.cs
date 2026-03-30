using System;
using UnityEngine;

namespace AI.FSM
{
    public abstract class BaseState<E_State, Manager>
        where E_State : Enum
        where Manager : StateManager<E_State>
    {
        public BaseState(E_State key, Manager manager)
        {
            StateKey = key;
            customerManager = manager;
        }
        
        public E_State StateKey {get; private set;}
        public Manager customerManager;
        public E_State nextState;
       
		public delegate bool PredicateFunc();
        public PredicateFunc stateCheck;

        public abstract void EnterState();

        public virtual void UpdateState()
        {
            if (stateCheck == null) return;
            
            if (stateCheck.Invoke())
            {
                customerManager.TransitionToState(nextState);
            }
        }
        public abstract void FixedUpdateState();
        public abstract void ExitState();

        public virtual void SetTransitionToState(E_State nextState, PredicateFunc stateCheck)
        {
            this.nextState = nextState;
            this.stateCheck = stateCheck;
        }
        
        public virtual void SetTransitionToState(E_State nextState)
        {
            this.nextState = nextState;
        }
    }
}
