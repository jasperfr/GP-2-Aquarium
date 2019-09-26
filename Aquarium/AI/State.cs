using Aquarium.Instances;
using System;

namespace Aquarium.AI
{
    public class State
    {
        public StateMachine StateMachine;
        public Action<GameInstance> Enter;
        public Action<GameInstance> Execute;
        public Action<GameInstance> Exit;
    }
}
