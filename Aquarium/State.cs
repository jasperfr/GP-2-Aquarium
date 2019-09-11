using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquarium
{
    public class StateMachine
    {
        public State CurrentState;

        public void SetState(State state)
        {
            CurrentState?.Exit?.Invoke();
            CurrentState = state;
            state.Enter?.Invoke();
        }

        public void Update()
        {
            CurrentState?.Execute();
        }
    }

    public class State
    {
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
