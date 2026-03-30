using System;
using System.Collections.Generic;
using Characters.Customer.CustomerState;
using UnityEngine;

namespace AI.FSM
{
    public abstract class StateManager<E_State> : MonoBehaviour where E_State : Enum
    {
        public Dictionary<E_State, BaseState<E_State, StateManager<E_State>>> States = new();
        public BaseState<E_State, StateManager<E_State>> CurrentState;

        protected virtual void Start()
        {
            CurrentState.EnterState();
        }

        protected virtual void Update()
        {
            CurrentState.UpdateState();
        }

        protected virtual void FixedUpdate()
        {
            CurrentState.FixedUpdateState();
        }

        public void AddState(BaseState<E_State, StateManager<E_State>> state)
        {
            States.Add(state.StateKey, state);
        }
        
        public void TransitionToState(E_State state)
        {
            CurrentState.ExitState();
            CurrentState = States[state];
            CurrentState.EnterState();
        }
    }
}