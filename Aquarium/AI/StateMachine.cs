using Aquarium.Instances;

namespace Aquarium.AI
{
    public class StateMachine
    {
        public GameInstance Entity;
        public State State;

        public StateMachine(State beginState)
        {
            State = beginState;
        }

        public void Start()
        {
            State.StateMachine = this;
            State.Enter?.Invoke(Entity);
        }
        public void SetState(State state)
        {
            State.StateMachine = this;
            State?.Exit?.Invoke(Entity);
            State = state;
            State.StateMachine = this;
            State.Enter?.Invoke(Entity);
        }
        public void UpdateState()
        {
            State.StateMachine = this;
            State?.Execute?.Invoke(Entity);
        }
    }
}
