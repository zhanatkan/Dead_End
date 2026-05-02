using VContainer;

namespace Game.Scripts.Base.States
{
    public sealed class InitServicesState : IState
    {
        private readonly StateMachine _stateMachine;

        [Inject]
        public InitServicesState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            InitServices();
        }

        public void Exit()
        {

        }

        private async void InitServices()
        {
            //_stateMachine.Enter<MainMenuState, SceneName>(SceneName.MainMenu);
        }
    }
}