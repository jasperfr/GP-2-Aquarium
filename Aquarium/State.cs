using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquarium
{
    public class StateMachine
    {
        public Entity Target;
        public State CurrentState;

        public StateMachine(Entity entity)
        {
            Target = entity;
        }

        public void SetState(State state)
        {
            if(CurrentState != null) CurrentState.SM = this;
            CurrentState?.Exit?.Invoke();
            CurrentState = state;
            CurrentState.SM = this;
            CurrentState.Enter?.Invoke();
        }

        public void Update()
        {
            CurrentState.SM = this;
            CurrentState?.Execute();
        }
    }

    public class State
    {
        public StateMachine SM;

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
