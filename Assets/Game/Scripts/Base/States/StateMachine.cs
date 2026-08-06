using System;
using System.Collections.Generic;
using VContainer;

namespace Game.Scripts.Base.States
{
    public sealed class StateMachine
    {
        private readonly IObjectResolver _container;
        
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        [Inject]
        public StateMachine(IObjectResolver container)
        {
            _container = container;
        }
        
        public void Initialize()
        {
            var factory = _container.Resolve<StatesFactory>();
            _states = factory.Create();
        }

        public void Enter<T>() where T : class, IState 
        {
            var state = ChangeState<T>();
            state.Enter();
        }

        public void Enter<T, TP>(TP payload) where T : class, IPayloadedState<TP> 
        {
            var state = ChangeState<T>();
            state.Enter(payload);
        }

        private T ChangeState<T>() where T : class, IExitableState 
        {
            _activeState?.Exit();

            var state = GetState<T>();
            _activeState = state;

            return state;
        }

        private T GetState<T>() where T : class, IExitableState => _states[typeof(T)] as T;
    }
}