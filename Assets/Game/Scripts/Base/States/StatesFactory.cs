using System;
using System.Collections.Generic;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.SaveLoad;
using Game.Scripts.Base.Services.Settings;
using UnityEditor.SettingsManagement;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Base.States
{
    public class StatesFactory
    {
        private readonly LifetimeScope _projectScope;
        private readonly StateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ISettingsProvider _settingsProvider;
        private readonly ISaveDataHandler _saveDataHandler;
        private readonly ISaveLoadService _saveLoadService;

        [Inject]
        public StatesFactory(LifetimeScope projectScope, StateMachine stateMachine, SceneLoader sceneLoader,
            ISettingsProvider settingsProvider, ISaveDataHandler saveDataHandler, ISaveLoadService saveLoadService)
        {
            _projectScope = projectScope;
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _settingsProvider = settingsProvider;
            _saveDataHandler = saveDataHandler;
            _saveLoadService = saveLoadService;
        }

        public Dictionary<Type, IExitableState> Create()
        {
            return new Dictionary<Type, IExitableState>
            {
                [typeof(LoadSettingsState)] =
                    new LoadSettingsState(_stateMachine, _settingsProvider),
                [typeof(LoadProgressState)] =
                    new LoadProgressState(_stateMachine, _settingsProvider,
                        _saveDataHandler, _saveLoadService),
                
            };
        }
    }
}