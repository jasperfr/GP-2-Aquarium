using System;

namespace Aquarium
{
    public class StateMachine
    {
        public GameObject Entity;
        public State CurrentState;

        public StateMachine(GameObject entity)
        {
            Entity = entity;
        }

        public void SetState(State state)
        {
            if(CurrentState != null) CurrentState.Handle = this;
            CurrentState?.Exit?.Invoke();
            CurrentState = state;
            CurrentState.Handle = this;
            CurrentState.Enter?.Invoke();
        }

        public void Update()
        {
            CurrentState.Handle = this;
            CurrentState?.Execute();
        }
    }

    public class State
    {
        public StateMachine Handle;

        public Action Enter;
        public Action Execute;
        public Action Exit;

        public State() {}
        public State(Action enter, Action execute, Action exit)
        {
            Enter = enter;
            Execute = execute;
            Exit = exit;
        }
    }
}
