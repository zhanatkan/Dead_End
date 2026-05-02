using System;
using Game.Scripts.Base.Services.Settings;
using VContainer;

namespace Game.Scripts.Base.States
{
    public sealed class LoadSettingsState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly ISettingsProvider _settingsProvider;

        [Inject]
        public LoadSettingsState(StateMachine stateMachine, ISettingsProvider settingsProvider)
        {
            _stateMachine = stateMachine;
            _settingsProvider = settingsProvider;
        }

        public void Enter()
        {
            LoadConfigs(GoToNextState);
        }

        public void Exit()
        {

        }

        private void LoadConfigs(Action onComplete)
        {
            _settingsProvider.LoadSettings(onComplete);
        }

        private void GoToNextState()
        {
            _stateMachine.Enter<LoadProgressState>();
        }
    }
}